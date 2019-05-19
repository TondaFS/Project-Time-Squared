using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InspectorLever))]
public class LeverEditor : Editor {

    public override void OnInspectorGUI()
    {
        InspectorLever b = (InspectorLever)target;

        if (b.commonTargets == null)
            b.commonTargets = new List<GameObject>();

        if (b.pastTargets == null)
            b.pastTargets = new List<GameObject>();

        if (b.presentTargets == null)
            b.presentTargets = new List<GameObject>();


        b.areTargetsCommon = EditorGUILayout.Toggle("Uses common targets: ", b.areTargetsCommon);
        b.leverChangeVelocity = EditorGUILayout.FloatField("Lever velocity", b.leverChangeVelocity);
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


            EditorGUILayout.Separator();
            b.pastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", b.pastMaterial, typeof(Material));
            b.isPastActive = EditorGUILayout.Toggle("Is Past Active", b.isPastActive);

        }
        else
        {
            b.presentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", b.presentMaterial, typeof(Material));
            b.isPresentActive = EditorGUILayout.Toggle("Is Present Active", b.isPresentActive);

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
