using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tirda reprezentujici Preassure plate.
/// </summary>
public class PreassurePlate : Activator {

    /// <summary>
    /// Pokud hrac vstoupi na pole s preassure plate, zapne jej.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == PTSLayers.Player)
        {
            OnActivate();
        }
    }
    /// <summary>
    /// Pokud hrac odejde z pole s preassure plate, vypne jej
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == PTSLayers.Player)
        {
            OnActivate();
        }
    }

    public void CreatePreassurePlate(TimelineObject type, Material mat, List<GameObject> targets)
    {
        ItemType = type;                
        material = mat;
        goTargets = targets;
        IsActivated = false;
        OccupyTile = false;
        Debug.Log("PP mat:" + mat);
    }

    protected override void CheckOnStart(Tile[] t)
    {
        base.CheckOnStart(t);

        GetComponent<BoxCollider>().isTrigger = true;
    }

}
