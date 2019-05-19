using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InspectorPreassurePlate))]
public class PreassurePlateEditor : Editor {

    public override void OnInspectorGUI()
    {
        InspectorPreassurePlate b = (InspectorPreassurePlate)target;

        if (b.commonTargets == null)
            b.commonTargets = new System.Collections.Generic.List<GameObject>();

        if (b.pastTargets == null)
            b.pastTargets = new System.Collections.Generic.List<GameObject>();

        if (b.presentTargets == null)
            b.presentTargets = new System.Collections.Generic.List<GameObject>();

        /*
        if (b.IsMain)
        {
            EditorGUILayout.HelpBox("Tento objekt je momentálně nastaven jako MAIN.", MessageType.Info);
            if (GUILayout.Button("Nastav druhý objekt jako MAIN"))
            {
                b.IsMain = false;
                b.otherTimelineRef.IsMain = true;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Tento objekt momentálně není nastaven jako MAIN", MessageType.Error);
            if (GUILayout.Button("NAstav tento objekt jako MAIN"))
            {
                b.IsMain = true;
                b.otherTimelineRef.IsMain = false;
            }
        }
        */
        b.areTargetsCommon = EditorGUILayout.Toggle("Uses common targets: ", b.areTargetsCommon);

        if (b.areTargetsCommon)
        {
            int count = Mathf.Max(0, EditorGUILayout.DelayedIntField("Common Targets", b.commonTargets.Count));
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
            b.alternativeMeshPresent = (GameObject)EditorGUILayout.ObjectField("Alternative PRESENT mesh", b.alternativeMeshPresent, typeof(GameObject));

            EditorGUILayout.Separator();
            b.pastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", b.pastMaterial, typeof(Material));
            b.alternativeMeshPast = (GameObject)EditorGUILayout.ObjectField("Alternative PAST mesh", b.alternativeMeshPast, typeof(GameObject));

        }
        else
        {
            b.presentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.presentMaterial, typeof(Material));
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
            
            b.pastMaterial = (Material)EditorGUILayout.ObjectField("Past Material",b.pastMaterial, typeof(Material));
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
        
        /*
        EditorGUILayout.Space();

        b.ItemType = (TimelineObject)EditorGUILayout.EnumPopup("Item Type: ", b.ItemType);
        b.IsVisible = EditorGUILayout.Toggle("Is Visible: ", b.IsVisible);

        if (b.ItemType.Equals(TimelineObject.Present))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //b.PresentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.PresentMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        else if (b.ItemType.Equals(TimelineObject.Past))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //b.PastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", b.PastMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }

#pragma warning disable CS0618 // Type or member is obsolete
        b.otherTimelineRef = (StateObject)EditorGUILayout.ObjectField("Other Timeline Reference", b.otherTimelineRef, typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete

        EditorGUILayout.Space();

        b.IsActivated = EditorGUILayout.Toggle("Is Activated", b.IsActivated);
        //b.OccupyTile = EditorGUILayout.Toggle("Occupy Tile", b.OccupyTile);

        
        int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Target References", b.TargetReferences.Count));
        while (newCount < b.TargetReferences.Count)
            b.TargetReferences.RemoveAt(b.TargetReferences.Count - 1);
        while (newCount > b.TargetReferences.Count)
            b.TargetReferences.Add(null);

        for (int i = 0; i < b.TargetReferences.Count; i++)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            b.TargetReferences[i] = (StateObject)EditorGUILayout.ObjectField(b.TargetReferences[i], typeof(StateObject));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        */
    }
}
