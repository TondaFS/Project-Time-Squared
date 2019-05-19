using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MovableObject {
    /// <summary>
    /// Urcuje, jestli je generator prave zaply nebo ne
    /// </summary>
    public bool IsOn = false;
    /// <summary>
    /// List vsech tilu, ktere Generator ovlivni 
    /// </summary>
    public List<Tile> AffectedTiles;
    /// <summary>
    /// Vzdalenost, ve ktere budou ovlivneny polem vsechny Tily okolo - osa Z
    /// </summary>
    public int AreaOfEffect = 1;
    /// <summary>
    /// Ovlivnuje Tile, na kterem se nachazi? Aka, zabira na nem misto => nelze umistovat jine Itemy
    /// </summary>
    public bool affectsTheTile = true;
    /// <summary>
    /// Je generator stale zaply
    /// </summary>
    public bool isAlwaysOn;
    /// <summary>
    /// Reference na FieldController
    /// </summary>
    FieldController field;
    /// <summary>
    /// Coroutine bezici pri aktivovani objektu
    /// </summary>
    Coroutine activateCoroutine;

    /// <summary>
    /// Reference pro všechny present tily
    /// </summary>
    List<Tile> present;
    /// <summary>
    /// Reference pro vsechny past tily
    /// </summary>
    List<Tile> past;

    /// <summary>
    /// Doba za kterou se pole rozsiri na maximum
    /// </summary>
    public float expansionTime;

    AudioSource audioSource;


    /// <summary>
    /// Nastavi vsechny hodnoty generatoru
    /// </summary>
    /// <param name="type"></param>
    /// <param name="presMat"></param>
    /// <param name="pastMat"></param>
    /// <param name="bothMat"></param>
    /// <param name="isOn"></param>
    /// <param name="areaOfEffect"></param>
    /// <param name="pickable"></param>
    /// <param name="pushable"></param>
    public void CreateGenerator(TimelineObject type, Material mat, bool isOn, int areaOfEffect, bool pickable, bool pushable, float expansionTime, bool alwaysOn)
    {
        ItemType = type;
        material = mat;
        IsOn = isOn;
        AreaOfEffect = areaOfEffect;
        isPickable = pickable;
        isPushable = pushable;
        ThrowableDistance = 2;
        movementVelocity = 5;     
        this.expansionTime = expansionTime;
        isAlwaysOn = alwaysOn;

        StartCoroutine(WaitForLevelInit());
    }

    IEnumerator WaitForLevelInit()
    {
        while (Level.Instance == null)
            yield return null;

        GameObject go = Instantiate(Level.Instance.generatorPrefab);
        go.transform.parent = transform;
        go.transform.localPosition = new Vector3(0.5f, 0, -0.5f);
        Debug.Log("Generator created");

        field = GetComponentInChildren<FieldController>();
        field.Radius = 0;
    }


    protected override void Start()
    {
        base.Start();

        Level.Instance.AddGeneratorToList(this);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SoundsManager.instance.generator;
        audioSource.loop = true;

        if (AffectedTiles == null)
            AffectedTiles = new List<Tile>();

        UpdateEffectedTiles();

        if (IsOn)
            StartCoroutine(WaitForItemsInit());

        CreateTileReferenceLists();
    }

    /// <summary>
    /// Vytvori listy s tily pro present a past
    /// </summary>
    void CreateTileReferenceLists()
    {
        present = new List<Tile>();
        past = new List<Tile>();

        foreach (Tile t in Level.Instance.PresentLevelTiles)
        {
            present.Add(t);
        }
        foreach (Tile t in Level.Instance.PastLevelTiles)
        {
            past.Add(t);
        }
    }

    IEnumerator WaitForItemsInit()
    {
        while (!Level.Instance.itemsLoaded)
            yield return null;

        IsOn = false;
        Debug.Log("All Items loaded... Activating Generator");
        Activate();
    }

    /// <summary>
    /// Metoda pro item na zaklade jeho casoveho zarazeni, nastavi ulozeni reference do 
    /// odpovidajiciho tilu a nastavi collidery a viditelnost.
    /// </summary>
    /// <param name="t"></param>
    protected override void CheckOnStart(Tile[] t)
    {        
        ChangeMaterial(PTSShaderEnum.Present);

        if (material != null)
            render.material = material;

        t[0].ObjectOnTile = this;
        t[0].IsOccupied = true;
        t[1].ObjectOnTile = this;
        t[1].IsOccupied = true;
        SetVisibleLayer();           
    }

    /// <summary>
    /// Vypne/zapne generator a dle toho pak upravi v kazdem ovivnenem tilu vsechny Ïtemy:
    /// tzn. zapne/vypne collidery a necha zmizet.
    /// </summary>
    public override void Activate()
    {
        IsOn = !IsOn;
        Debug.Log("Generator SWITCHED to " + IsOn);

        if (activateCoroutine != null)
            StopCoroutine(activateCoroutine);
        activateCoroutine = StartCoroutine(ToggleField());

        SwitchTiles(AffectedTiles);
    }

    void SwitchTiles(List<Tile> tiles)
    {
        foreach (Tile t in tiles)
        {
            PreassurePlate pp = t.ActivatorOnTile as PreassurePlate;
            if (pp != null)
                pp.ChangeState();

            if (t.LaserOnTile != null)
                t.LaserOnTile.ChangeState();

            if (t.ObjectOnTile != null)
            {
                Generator g = t.ObjectOnTile as Generator;
                if (g != null)
                    continue;

                //Debug.Log("Vyska generatoru: " + (int)transform.position.y);

                if (Mathf.Abs((int)transform.position.y - (int)t.ObjectOnTile.transform.position.y) <= AreaOfEffect)
                    t.ObjectOnTile.ChangeState();
                
                Box b = t.ObjectOnTile as Box;
                while (b != null && b.ItemOnTop != null)
                {
                    if (Mathf.Abs((int)transform.position.y - (int)b.ItemOnTop.transform.position.y) <= AreaOfEffect)
                        b.ItemOnTop.ChangeState();
                    b = b.ItemOnTop as Box;
                }
            }
        }
    }

    private IEnumerator ToggleField()
    {
        float currentTime = 0;
        float len = 1/(expansionTime * 2);

        if (IsOn)
        {
            audioSource.volume = 1;
            audioSource.Play();
            while (field.Radius < AreaOfEffect)
            {
                currentTime += Time.deltaTime;

                float t = currentTime;
                t = Mathf.Sin(t * Mathf.PI * 0.5f * len);

                field.Radius = Mathf.Lerp(0.01f, AreaOfEffect + 1, t);

                yield return null;
            }
        }
        else
        {
            StartCoroutine(FadeOut());
            while (field.Radius > 0.02f)
            {
                currentTime += Time.deltaTime;

                float t = currentTime;
                t = Mathf.Sin(t * Mathf.PI * 0.5f * len);

                field.Radius = Mathf.Lerp(AreaOfEffect, 0.01f, t);

                yield return null;
            }
            field.Radius = 0;            
        }

        yield return null;
    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
    }

    /// <summary>
    /// Znovu si prepocita tily, ktere bude ovlivnovat
    /// </summary>
    public void UpdateEffectedTiles()
    {
        //Debug.Log("Updating effected tiles");
        AffectedTiles.Clear();
        Tile[] t = Level.GetTile(transform.position);
        AffectedTiles = Level.Instance.GetAllTilesAtDistance(t[0], AreaOfEffect);
    }

    /// <summary>
    /// Prepne stavy vsech itemu ve hre
    /// </summary>
    public void SwitchAllObjects()
    {    
        SwitchTiles(present);
        SwitchTiles(past);
    }
    
    /// <summary>
    /// Kdyz je generator sebran a je aktivni
    /// </summary>
    public void OnActivePickable()
    {
        SwitchTiles(AffectedTiles);
        SwitchAllObjects();
    }

    /// <summary>
    /// Kdyz je generator polozen na zem a je aktivni
    /// </summary>
    public void OnActiveDropable()
    {
        SwitchAllObjects();
        //UpdateEffectedTiles();
        SwitchTiles(AffectedTiles);
    }

    /// <summary>
    /// Aktivace generatoru behem toho, kdy ho hrac nese
    /// </summary>
    public void ActivateOnPick()
    {
        IsOn = !IsOn;
        Debug.Log("Generator SWITCHED to " + IsOn);

        if (activateCoroutine != null)
            StopCoroutine(activateCoroutine);
        activateCoroutine = StartCoroutine(ToggleField());

        SwitchAllObjects();
    }
}
