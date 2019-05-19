using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAvaliableTile : MonoBehaviour {
    public TimelineObject timeline;

	void Start () {
        Tile[] t = Level.GetTile(transform.position);

        if (timeline.Equals(TimelineObject.Present))
            t[0].IsAccessable = false;
        else if (timeline.Equals(TimelineObject.Past))
            t[1].IsAccessable = false;
        else if (timeline.Equals(TimelineObject.Both))
        {
            t[0].IsAccessable = false;
            t[1].IsAccessable = false;
        }

        Debug.Log("Tile " + t[0].Position + " not Avaliable");
	}
}
