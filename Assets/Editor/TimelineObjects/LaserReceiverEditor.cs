using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaserReceiverInspector))]
public class LaserReceiverEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        LaserReceiverInspector b = (LaserReceiverInspector)target;

        b.timeline = (TimelineObject)EditorGUILayout.EnumPopup("Timeline: ", b.timeline);

        if (b.targets == null)
            b.targets = new System.Collections.Generic.List<GameObject>();

        /*
        if (b.IsMain)
        {
            EditorGUILayout.HelpBox("This object timeline is currently MAIN.", MessageType.Info);
            if (GUILayout.Button("Set the other timeline object to MAIN"))
            {
                b.IsMain = false;
                b.otherTimelineRef.IsMain = true;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("This object timeline is NOT MAIN", MessageType.Error);
            if (GUILayout.Button("Set this timeline object to MAIN"))
            {
                b.IsMain = true;
                b.otherTimelineRef.IsMain = false;
            }
        }
        */

        b.occupyTile = EditorGUILayout.Toggle("Occupies Tile", b.occupyTile);
        EditorGUILayout.Separator();

            int count = Mathf.Max(0, EditorGUILayout.DelayedIntField("Common Targets", b.targets.Count));
            //EditorGUILayout.TextArea("Nejaky to text: " + count);

            while (count < b.targets.Count)
                b.targets.RemoveAt(b.targets.Count - 1);
            while (count > b.targets.Count)
                b.targets.Add(null);
            
            for (int i = 0; i < b.targets.Count; i++)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                b.targets[i] = (GameObject)EditorGUILayout.ObjectField("Target " + i, b.targets[i], typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete
            }

            EditorGUILayout.Separator();
            b.presentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.presentMaterial, typeof(Material));
            b.isPresentActive = EditorGUILayout.Toggle("Is Present Active", b.isPresentActive);
            b.alternativeMeshPresent = (GameObject)EditorGUILayout.ObjectField("Alternative PRESENT mesh", b.alternativeMeshPresent, typeof(GameObject));
    

        

    }
}
