using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InspectorButton))]
public class ButtonEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        InspectorButton b = (InspectorButton)target;

        if (b.commonTargets == null)
            b.commonTargets = new System.Collections.Generic.List<GameObject>();

        if (b.pastTargets == null)
            b.pastTargets = new System.Collections.Generic.List<GameObject>();

        if (b.presentTargets == null)
            b.presentTargets = new System.Collections.Generic.List<GameObject>();

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

        b.areTargetsCommon = EditorGUILayout.Toggle("Uses common targets: ", b.areTargetsCommon);
        b.occupyTile = EditorGUILayout.Toggle("Occupies Tile", b.occupyTile);
        EditorGUILayout.Separator();

        if (b.areTargetsCommon)
        {
            int count = Mathf.Max(0, EditorGUILayout.DelayedIntField("Common Targets", b.commonTargets.Count));
            //EditorGUILayout.TextArea("Nejaky to text: " + count);

            while (count < b.commonTargets.Count)
                b.commonTargets.RemoveAt(b.commonTargets.Count - 1);
            while (count > b.commonTargets.Count)
                b.commonTargets.Add(null);
            
            for (int i = 0; i < b.commonTargets.Count; i++)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                b.commonTargets[i] = (GameObject)EditorGUILayout.ObjectField("Target " + i, b.commonTargets[i], typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete
            }

            EditorGUILayout.Separator();
            b.presentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.presentMaterial, typeof(Material));
            b.isPresentActive = EditorGUILayout.Toggle("Is Present Active", b.isPresentActive);
            b.alternativeMeshPresent = (GameObject)EditorGUILayout.ObjectField("Alternative PRESENT mesh", b.alternativeMeshPresent, typeof(GameObject));


            EditorGUILayout.Separator();
            b.pastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", b.pastMaterial, typeof(Material));
            b.isPastActive = EditorGUILayout.Toggle("Is Past Active", b.isPastActive);
            b.alternativeMeshPast = (GameObject)EditorGUILayout.ObjectField("Alternative PAST mesh", b.alternativeMeshPast, typeof(GameObject));

        }
        else
        {
            b.presentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.presentMaterial, typeof(Material));
            b.isPresentActive = EditorGUILayout.Toggle("Is Present Active", b.isPresentActive);
            b.alternativeMeshPresent = (GameObject)EditorGUILayout.ObjectField("Alternative PRESENT mesh", b.alternativeMeshPresent, typeof(GameObject));


            int count = Mathf.Max(0, EditorGUILayout.DelayedIntField("Present Targets", b.presentTargets.Count));
            while (count < b.presentTargets.Count)
                b.presentTargets.RemoveAt(b.presentTargets.Count - 1);
            while (count > b.presentTargets.Count)
                b.presentTargets.Add(null);

            for (int i = 0; i < b.presentTargets.Count; i++)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                b.presentTargets[i] = (GameObject)EditorGUILayout.ObjectField("Target " + i, b.presentTargets[i], typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete
            }

            EditorGUILayout.Space();

            b.pastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", b.pastMaterial, typeof(Material));
            b.isPastActive = EditorGUILayout.Toggle("Is Past Active", b.isPastActive);
            b.alternativeMeshPast = (GameObject)EditorGUILayout.ObjectField("Alternative PAST mesh", b.alternativeMeshPast, typeof(GameObject));


            int countPast = Mathf.Max(0, EditorGUILayout.DelayedIntField("Past Targets", b.pastTargets.Count));
            while (countPast < b.pastTargets.Count)
                b.pastTargets.RemoveAt(b.pastTargets.Count - 1);
            while (countPast > b.pastTargets.Count)
                b.pastTargets.Add(null);

            for (int i = 0; i < b.pastTargets.Count; i++)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                b.pastTargets[i] = (GameObject)EditorGUILayout.ObjectField("Target " + i, b.pastTargets[i], typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        

    }
}
