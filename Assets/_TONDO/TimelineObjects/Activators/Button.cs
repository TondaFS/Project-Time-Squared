using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Activator {

    public override void Activate()
    {
        IsActivated = false;
        StartCoroutine(WaitForItemsInit());
    }

    public void CreateButton(TimelineObject type, Material mat, List<GameObject> targets, bool isActive, bool occupyTile)
    {
        ItemType = type;
        material = mat;
        goTargets = targets;
        IsActivated = isActive;
        OccupyTile = occupyTile;
    }

    

}
