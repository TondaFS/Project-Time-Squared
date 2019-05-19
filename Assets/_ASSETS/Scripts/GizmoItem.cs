using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class GizmoItem : MonoBehaviour {
    string text;
    Tile t;
    public GUIStyle textStyle;


    void Start() {
        StartCoroutine(ShowGizmo());
    }

    IEnumerator ShowGizmo()
    {
        yield return new WaitForSeconds(2);

        Vector2Int v = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        t = Level.GetTile(v)[0];
    }

    void OnDrawGizmos()
    {
        text = "";

        if (t != null)
        {            
            if (t.ObjectOnTile != null)
                text += "Present: " + t.ObjectOnTile.name + "\n";
            if (t.OtherTimelineRef.ObjectOnTile != null)
                text += "Past: " + t.OtherTimelineRef.ObjectOnTile.name;

            if (t.ActivatorOnTile != null)
                text += "\n Present Activator: " + t.ActivatorOnTile.name;
            if (t.OtherTimelineRef.ActivatorOnTile != null)           
                text += "\n Past Activator: " + t.OtherTimelineRef.ActivatorOnTile.name;
        }
        UnityEditor.Handles.Label(transform.position - new Vector3(0, 0, 0), text, textStyle);
    }
}
#endif
