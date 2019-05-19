using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Trida reprezentujici jakykoliv object Projet Time Squared, ktery spada do
/// nejake casove linie, popr. obou.
/// </summary>
public class PTSObject : MonoBehaviour {
    /// <summary>
    /// Typ objektu, ktery urcuje, od jake timeline patri 
    /// </summary>
    public TimelineObject ItemType;
    /// <summary>
    /// Je objekt viditelny a ma zaple collidery pro hrace?
    /// </summary>
    public bool IsVisible = true;    
    /// <summary>
    /// Reference na mesh renderer tohoto predmetu
    /// </summary>
    public MeshRenderer render;
    /// <summary>
    /// material objektu
    /// </summary>
    public Material material;

    protected virtual void Start()
    {
        render = GetComponent<MeshRenderer>();
        Tile[] t = Level.GetTile(transform.position);
        CheckOnStart(t);
    }

    /// <summary>
    /// Zmeni stav objektu na opacny: tzn. z viditelneho na neviditelny a naopak.
    /// </summary>
    public void ChangeState()
    {
        if (IsVisible)
            SetInvisibleLayer();
        else
            SetVisibleLayer();
    }
    /// <summary>
    /// Nastavi objektu vrstvu, ktera nekoliduje s hracem
    /// </summary>
    public void SetInvisibleLayer()
    {
        gameObject.layer = PTSLayers.NoInteractionWithPlayer;
        IsVisible = false;
    }
    /// <summary>
    /// Nastavi objektu vrstvu, ktera koliduje s hracem
    /// </summary>
    public void SetVisibleLayer()
    {
        gameObject.layer = PTSLayers.InteractionWithPlayer;
        IsVisible = true;
    }

    public void ChangeMaterial(float timeline)
    {
        render.material.SetFloat("_Timeline", timeline);
        
        if (timeline == PTSShaderEnum.Present)
        {
            render.material.DisableKeyword("_TIMELINE_STAY");
            render.material.EnableKeyword("_TIMELINE_PRESENT");
            render.material.DisableKeyword("_TIMELINE_PAST");
        } else if (timeline == PTSShaderEnum.Stay)
        {
            render.material.EnableKeyword("_TIMELINE_STAY");
            render.material.DisableKeyword("_TIMELINE_PRESENT");
            render.material.DisableKeyword("_TIMELINE_PAST");
        } else if (timeline == PTSShaderEnum.Past)
        {
            render.material.DisableKeyword("_TIMELINE_STAY");
            render.material.DisableKeyword("_TIMELINE_PRESENT");
            render.material.EnableKeyword("_TIMELINE_PAST");
        }        
    }

    /// <summary>
    /// Metoda, ktera bude zavolana, pokud lze dany objket nejakym zpusobem aktivovat. Kazdy objekt,
    /// musi mit napsanou svou vlastni Activate metodu
    /// </summary>
    public virtual void Activate()
    {
        Debug.Log("Object Base activation");
        //vymyslet kam jinam tuto metodu hodit, aby se na ni mohli aktivatory odkazovat...
    }

    /// <summary>
    /// Pri nacteni sceny kontroluje zakladni stav, na zaklade generatoru v okoli a toho
    /// zda se jedna nebo nejedna o main objekt
    /// </summary>
    /// <param name="t"></param>
    protected virtual void CheckOnStart(Tile[] t)
    {
        if (ItemType.Equals(TimelineObject.Present))
        {
            t[0].ObjectOnTile = this;
            t[0].IsOccupied = true;            
            SetVisibleLayer();
            //Debug.Log("PTSObject to tile " + t[0].Position);
        }
        else if (ItemType.Equals(TimelineObject.Past))
        {
            t[1].ObjectOnTile = this;
            t[1].IsOccupied = true;
            SetInvisibleLayer();
            //Debug.Log("PTSObject to tile " + t[1].Position);
        }
    }
}

/// <summary>
/// Typ objektu, urcujici, do jake casove linie spada
/// </summary>
public enum TimelineObject { Present, Past, Both }
