using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třída reprezentující celý level jako 2D pole s tily v přítomnosti i minulosti
/// Hlavní idea je, že levý spodní roh celého levelu je umístěn v (0,0,0) 
/// </summary>
public class Level : MonoBehaviour {
    public static Level Instance;
    public GameObject generatorPrefab;
    /// <summary>
    /// Seznam itemů ve scéně
    /// </summary>
    public List<Item> Items;
    /// <summary>
    /// Seznam Generátorů ve scéně
    /// </summary>
    public List<Generator> Generators;
    public List<Door> Doors;
    public List<Activator> Activators;
    public List<StaticGeometry> StaticObjects;

    /// <summary>
    /// Pocet anomalii, ktere hreac musi vyresit, nez bude moci znova prepnout generator
    /// </summary>
    public int anomalyCounter = 0;
        
    /// <summary>
    /// 2D pole reprezentující celý level v přítomnosti
    /// </summary>
    public Tile[,] PresentLevelTiles { get; set; }
    /// <summary>
    /// 2D pole reprezentující celý level v minulosti
    /// </summary>
    public Tile[,] PastLevelTiles { get; set; }
    
    public int levelWidth = 30;
    public int levelHeight = 30;

    int ptsObjectsInScene = 0;
    int ptsObjectsLoaded = 0;
    public bool itemsLoaded = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        CreateNetwork();
        CheckTileSystem(PresentLevelTiles);
        CheckTileSystem(PastLevelTiles);

        Shader.SetGlobalVector("_FieldPos", new Vector3(-1,-1,-1));
        Shader.SetGlobalFloat("_FieldRad", 0);
    }

    private void Start()
    {
        ptsObjectsInScene = FindObjectsOfType(typeof(PTSObject)).Length;
        //Debug.Log(ptsObjectsInScene);
        StartCoroutine(CheckItemsLoaded());
    }

    public void CheckTileSystem(Tile[,] tiles)
    {
        int errors = 0;
        for (int i = 0; i < tiles.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < tiles.GetLength(1) - 1; j++)
            {
                Tile t = tiles[i, j];
                

                if (t.Position.x != -i || t.Position.y != j)
                {
                    t.Position = new Vector2Int(-i, j);
                    errors++;
                }
                    
                
                //Debug.Log("POZICE: i=" + i + " j=" + j + " je x=" + tiles[i, j].Position.x + " y=" + tiles[i, j].Position.y);
            }
        }
        Debug.Log(errors + " errors corrected during setup check.");
    }

    /// <summary>
    /// Pocka dokud se neinicializuji vsechny itemy a pote nastavi itemsLoaded na true
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckItemsLoaded()
    {
        while (ptsObjectsLoaded != ptsObjectsInScene)
        {
            yield return null;
        }
        Debug.Log("All PTS objects loaded...");
        itemsLoaded = true;
    }

    /// <summary>
    /// Vytvori LevelTiles pole pro pritomnosti i munulost s danym poctem radku a sloupcu 
    /// </summary>
    /// <param name="rows">Pozadovany pocet radku</param>
    /// <param name="colums">Pozadovany pocet sloupcu</param>
    public void GenerateLevelTiles(int rows, int colums)
    {
        PresentLevelTiles = new Tile[rows, colums];
        PastLevelTiles = new Tile[rows, colums];
    }
    
    /// <summary>
    /// Vytvoří tilovou reprezentaci levelu
    /// </summary>
    public void CreateNetwork()
    {
        GenerateLevelTiles(levelWidth, levelHeight);

        Tile[,] lvlPresent = PresentLevelTiles;
        Tile[,] lvlPast = PastLevelTiles;

        Vector3 movChange = new Vector3(-0.5f, 0, 0.5f);

        Vector3 StartPos = new Vector3(-0.5f, 0, 0.5f);
        Vector3 xVector = new Vector3(-1, 0, 0);
        Vector3 zVector = new Vector3(0, 0, 1);

        GameObject cubesRef = new GameObject
        {
            name = "Reference Cubes container"
        };

        GameObject wallsRef = new GameObject
        {
            name = "WallsBeyondLevel"
        };

        cubesRef.transform.position = new Vector3(0, 0, 0);
        
        for (int i = -1; i < levelHeight; i++)
        {
            for (int j = -1; j < levelWidth; j++)
            {
                                
                Vector3 vertice = StartPos + i * xVector + j * zVector + new Vector3(0, 20, 0);
                int mask = LayerMask.GetMask(LayerMask.LayerToName(PTSLayers.Floor));
                Ray r = new Ray(vertice, new Vector3(0, -1, 0));
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 50, mask))
                {

                    GameObject o = Instantiate(Resources.Load("Highlight", typeof(GameObject))) as GameObject;
                    o.transform.position = hit.point;
                    o.transform.SetParent(cubesRef.transform);
                    o.name = "TILE: X" + i + "Z" + j;

                    lvlPresent[i, j] = new Tile(hit.point - movChange);
                    lvlPast[i, j] = new Tile(hit.point - movChange);
                    lvlPresent[i, j].referenceCube = o;
                    lvlPast[i, j].referenceCube = o;
                    lvlPresent[i, j].isPresentTile = true;
                    lvlPast[i, j].isPresentTile = false;
                    lvlPresent[i, j].OtherTimelineRef = lvlPast[i, j];
                    lvlPast[i, j].OtherTimelineRef = lvlPresent[i, j];
                    lvlPresent[i, j].Timeline = TimelineObject.Present;
                    lvlPast[i, j].Timeline = TimelineObject.Past;
                }
                else
                {  
                    GameObject go = new GameObject();
                    go.transform.position = vertice - new Vector3(0, 20, 0);
                    BoxCollider bc = go.AddComponent(typeof(BoxCollider)) as BoxCollider;
                    bc.center = new Vector3(0, 0, 0);
                    bc.size = new Vector3(1, 10, 1);
                    go.transform.parent = wallsRef.transform;

                    if (i < 0 || j < 0)
                        continue;

                    lvlPresent[i, j] = new Tile(new Vector3(i, 0, j));
                    lvlPast[i, j] = new Tile(new Vector3(i, 0, j));
                    lvlPresent[i, j].isPresentTile = true;
                    lvlPast[i, j].isPresentTile = false;
                    lvlPresent[i, j].IsAccessable = false;
                    lvlPast[i, j].IsAccessable = false;
                    lvlPresent[i, j].OtherTimelineRef = lvlPast[i, j];
                    lvlPast[i, j].OtherTimelineRef = lvlPresent[i, j];
                    lvlPresent[i, j].Timeline = TimelineObject.Present;
                    lvlPast[i, j].Timeline = TimelineObject.Past;
                }
            }
        }
    }

    /// <summary>
    /// Gets the appropriate tile from the level array based on the position in the scene
    /// </summary>
    /// <param name="position">Position in the scene</param>
    /// <returns></returns>
    public static Tile[] GetTile(Vector3 position)
    {
        //musime prevratit x hodnotu kvuli orientace levelu ve scene
        position.x = position.x * -1;
        if ((int)position.x < 0 || (int)position.x > Instance.PresentLevelTiles.GetLength(0) ||
            (int)position.z < 0 || (int)position.z > Instance.PresentLevelTiles.GetLength(1))
        {
            Debug.Log("Returning null");
            return null;
        }

        //Debug.Log("NO INT: " + position.x + " " + position.z);
        //Debug.Log("INTL " + (int)position.x + " " + (int)position.z);
        return new Tile[] {Instance.PresentLevelTiles[(int)position.x, (int)position.z],
            Instance.PastLevelTiles[(int)position.x, (int)position.z] };
    }
    public Tile[] GetTile(Tile t)
    {        
        return new Tile[] { PresentLevelTiles[-t.Position.x, t.Position.y], PastLevelTiles[-t.Position.x, t.Position.y] };
    }
    /// <summary>
    /// Gets the appropriate tile from the level array based on the position in the scene
    /// </summary>
    /// <param name="position">Position in the scene</param>
    /// <returns></returns>
    public static Tile[] GetTile(Vector2Int position)
    {
        //musime prevratit x hodnotu kvuli orientace levelu ve scene
        position.x = position.x * -1;
        if (position.x < 0 || position.x >= Instance.PresentLevelTiles.GetLength(0) ||
            position.y < 0 || position.y >= Instance.PresentLevelTiles.GetLength(1))
            return null;

        return new Tile[] {Instance.PresentLevelTiles[position.x, position.y],
            Instance.PastLevelTiles[position.x, position.y] };
    }

    public Tile[] GetNeighbours(Tile t)
    {
        int x = -t.Position.x;
        int z = t.Position.y;

        //Debug.Log("x: " + x + " and z: " + z);
        List<Tile> tiles = new List<Tile>();

        if (x >= 1)
            tiles.Add(PresentLevelTiles[x - 1, z]);

        if (z < PresentLevelTiles.GetLength(1) - 2)
            tiles.Add(PresentLevelTiles[x, z + 1]);

        if (x < PresentLevelTiles.GetLength(0) - 2)
            tiles.Add(PresentLevelTiles[x + 1,z]);

        if (z >= 1)
            tiles.Add(PresentLevelTiles[x, z - 1]);

        return tiles.ToArray();
    }

    public void ChangeTileAccessability(Vector3 position, TimelineObject to, bool change)
    {
        Tile[] t = GetTile(position);
        /*
        if (to.Equals(TimelineObject.Present))
            t[0].ChangeAccessability(change);
        else
            t[1].ChangeAccessability(change);
        */

        if (to.Equals(TimelineObject.Present))
            t[0].IsOccupied = change;
        else
            t[1].IsOccupied = change;
    }

    /// <summary>
    /// Prida item do listu itemu. Pokud list jeste nebyl inicializovan,
    /// inicializuje jej.
    /// </summary>
    /// <param name="i"></param>
    public void AddItemToList(Item i)
    {
        if (Items == null)
            Items = new List<Item>();

        Items.Add(i);
        ptsObjectsLoaded += 1;
    }
    public void AddDoorToList(Door d)
    {
        if (Doors == null)
            Doors = new List<Door>();

        Doors.Add(d);
        ptsObjectsLoaded += 1;

    }
    public void AddActivatorToList(Activator d)
    {
        if (Activators == null)
            Activators = new List<Activator>();

        Activators.Add(d);
        ptsObjectsLoaded += 1;

    }
    public void AddStaticGeometryToList(StaticGeometry d)
    {
        if (StaticObjects == null)
           StaticObjects = new List<StaticGeometry>();

        StaticObjects.Add(d);
        ptsObjectsLoaded += 1;

    }
    /// <summary>
    /// Prida generator do listu generatoru. Pokud nebzl list jeste inicializova, 
    /// inicializuje jej.
    /// </summary>
    /// <param name="g"></param>
    public void AddGeneratorToList(Generator g)
    {
        if (Generators == null)
            Generators = new List<Generator>();

        Generators.Add(g);
        ptsObjectsLoaded += 1;

    }

    /// <summary>
    /// Projde vsechny Tily v rozmezi width a height od vstupniho tilu a vrati je 
    /// jako list
    /// </summary>
    /// <param name="centerTile">Tile kolem ktereho zjistujeme Tily</param>
    /// <param name="width">Vzdalenost v Z-tove ose</param>
    /// <param name="height">(Nepovinne) Vzdalenost v X-ove ose. Pokud neni uvedeno, je hodnota natavena na 0
    /// => bude pouzita stejna hodnota jako width => ctvercova oblast kolem Tilu.</param>
    /// <returns></returns>
    public List<Tile> GetAllTilesAtDistance(Tile centerTile, int width, int height = 0)
    {
        List<Tile> l = new List<Tile>();

        if (height == 0)
            height = width;

        int ZLeftLimit = (int)centerTile.Position.y - width;
        int ZRightLimit = (int)centerTile.Position.y + width;

        if (ZLeftLimit < 0)
            ZLeftLimit = 0;
        
        if (ZRightLimit > PresentLevelTiles.GetLength(1) - 1)
            ZRightLimit = PresentLevelTiles.GetLength(1) - 1;

        int XBottomLimit = (int)centerTile.Position.x * (-1) - width;
        int XTopLimit = (int)centerTile.Position.x * (-1) + width;

        if (XBottomLimit < 0)
            XBottomLimit = 0;

        if (XTopLimit > PresentLevelTiles.GetLength(0) - 1)
            XTopLimit = PresentLevelTiles.GetLength(0) - 1;

    
        Debug.Log("LIMITS: X:(" + XBottomLimit + "," + XTopLimit + ") Z(" + ZLeftLimit + "," + ZRightLimit + ")");

        for (int i = XBottomLimit; i <= XTopLimit; i++)
        {
            for (int j = ZLeftLimit; j <= ZRightLimit; j++)
            {
                //Debug.Log(i + " " + j);
                Tile[] a = GetTile(new Vector3(i*-1, 0, j));
                //a.SetTileToHighlited();
                l.Add(a[0]);
                l.Add(a[1]);
                //Debug.Log("Tile added");
            }
        }
        
        return l;
    }
    
    /// <summary>
    /// Projde vsechny generatory a u nich vsechny tily, a pokud se vstupni tile
    /// rovna s kterymkoliv tile v tomto seznamu, prida generator do seznamu.
    /// Na konci vraci list generatoru
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public List<Generator> GetGeneratorsAffectingTile(Tile tile)
    {
        List<Generator> l = new List<Generator>();

        foreach(Generator g in Generators)
        {
            if (g.AffectedTiles == null)
                continue;

            foreach(Tile t in g.AffectedTiles)
            {
                if (t.Equals(tile))
                {
                    l.Add(g);
                    break;
                }                    
            }
        }

        return l;
    }

    /// <summary>
    /// Zkontroluje, jestli objekt, ktery prave pokladame na zem, urci, ktery tile (v min nebo pritomnosti)
    /// mame pouzit pro umisteni predmetu a kterou veryi predmetu umistujeme
    /// </summary>
    /// <param name="t"></param>
    public Tile CheckTimeLineDrop(Tile t, MovableObject o)
    {        
        List<Generator> gens = GetGeneratorsAffectingTile(t);

        //Nevypiname, pokud Objekt muze byt jen v pritomnosti a tile neovlivnuji zadne generatory
        if (gens.Count == 0)
        {
            Debug.Log("<<<DROP ON PRESENT>>>");
            o.ChangeMaterial(PTSShaderEnum.Present);
            return PresentLevelTiles[-t.Position.x, t.Position.y];
        }            
        else
        {
            foreach(Generator g in gens)
            { 
                if (g.IsOn)
                {
                    Debug.Log("<<<DROP ON PAST>>>");
                    o.ChangeMaterial(PTSShaderEnum.Past);
                    return PastLevelTiles[-t.Position.x, t.Position.y];
                }
                    
            }
            Debug.Log("<<<DROP ON PRESENT>>>");
            o.ChangeMaterial(PTSShaderEnum.Present); 
            return PresentLevelTiles[-t.Position.x, t.Position.y];
        }        
    }

        

        /*
        //Vypneme, pokud pokladame item co je jen v minulosti na pole, ktere neni ve vlivu
        //zadneho generatoru
        if (gens.Count == 0 && ItemType.Equals(TimelineObject.Past))
        {
            Debug.LogWarning("Gens is 0 and Item is Past / HIDING it");
            EnableGeneratorInfluence(null);
        }        
        
        //Vypneme v pripade, pokud najdeme alespon jeden generator je zaply/vyply
        //a jedna se o predmet typu Present/Past
        foreach(Generator g in gens)
        {
            if (ItemType.Equals(TimelineObject.Present) && g.IsOn)
            {
                Debug.LogWarning("Presen item and generator is on");
                EnableGeneratorInfluence(g);
            }           
            else if (ItemType.Equals(TimelineObject.Past) && !g.IsOn){
                Debug.LogWarning("Past item and generator off");
                EnableGeneratorInfluence(g);
            }                
        }
        */
}

/// <summary>
/// Dvojice Item a Tile
/// </summary>
public struct ObjectTilePair
{
    public MovableObject i;
    public Tile t;
}
