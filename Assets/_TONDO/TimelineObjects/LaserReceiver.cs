using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : Activator {

    public void CreateReceiver(TimelineObject timeline, Material mat, List<GameObject> targs, bool isPresActive,
        bool occupytile)
    {
        ItemType = timeline;
        material = mat;
        goTargets = targs;
        IsActivated = isPresActive;
        OccupyTile = occupytile;
    }

}
