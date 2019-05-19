using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Trida reprezentujici vsechny objekty, ktere mohou po pouziti vypinat, zapinat jine PTS objekty, jenz
/// maji funkcni Activate metodu.
/// </summary>
public class Activator : StateObject {
    protected List<GameObject> goTargets;
    /// <summary>
    /// Reference na vsechny objekty, ktere aktivator prepina/spousti
    /// </summary>
    public List<PTSObject> TargetReferences;
    /// <summary>
    /// Urcuje, zda je aktivator momentalne zaply
    /// </summary>
    public bool IsActivated;
    /// <summary>
    /// Urcuje, jestli aktivator zabira Tile, na kterem se nachazi a tedy
    /// pak blokuje umistovani jinych objektu a posunovani na nej.
    /// </summary>
    public bool OccupyTile; 

    protected override void Start()
    {
        base.Start();

        Level.Instance.AddActivatorToList(this);

        TargetReferences = new List<PTSObject>();

        if (goTargets != null)
        {
            foreach (GameObject go in goTargets)
            {
                PTSObject[] targets;
                if (go.transform.parent != null)
                    targets = go.transform.parent.GetComponentsInChildren<PTSObject>();
                else
                    targets = new PTSObject[] { go.GetComponent<PTSObject>()};

                foreach (PTSObject o in targets)
                {
                    if (o.ItemType.Equals(ItemType) || o.ItemType.Equals(TimelineObject.Both))
                    {
                        TargetReferences.Add(o);
                        continue;
                    }
                }
            }
        }

        if (IsActivated)
            Activate();
    }

    protected override void CheckOnStart(Tile[] t)
    {
        if (ItemType.Equals(TimelineObject.Present))
        {
            t[0].ActivatorOnTile = this;
            t[0].IsOccupied = OccupyTile;
            SetVisibleLayer();
        }
        else if (ItemType.Equals(TimelineObject.Past))
        {
            t[1].ActivatorOnTile = this;
            t[1].IsOccupied = OccupyTile;
            SetInvisibleLayer();
        }

        if (material != null)
            render.material = material;

        StartCoroutine(AssignToDoors());
    }

    /// <summary>
    /// Priradi se do seznamu aktivatoru, ktere dvere obladaji
    /// </summary>
    IEnumerator AssignToDoors()
    {
        while (TargetReferences == null)
            yield return null;

        foreach (PTSObject ob in TargetReferences)
        {
            Door d = ob as Door;
            if (d != null)
            {
                if (d.buttons == null)
                    d.buttons = new List<Activator>();

                d.buttons.Add(this);
            }
        }
    }

    /// <summary>
    /// Metoda, ktera se zavola, kdyz je Aktivator aktivovan.
    /// </summary>
    public virtual void OnActivate()
    {
        IsActivated = !IsActivated;

        foreach(PTSObject ob in TargetReferences)
        {
            ob.Activate();
        }

        SoundsManager.instance.PlayButtonPressed();
    }

    protected IEnumerator WaitForItemsInit()
    {
        while (!Level.Instance.itemsLoaded)
            yield return null;

        OnActivate();
    }
}
