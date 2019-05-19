using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Box))]
public class BoxEditor : Editor {

    public override void OnInspectorGUI()
    {
        Box objBaseScript = (Box)target;

        

        DrawDefaultInspector();
        /*
        if (objBaseScript.IsMain)
        {
            EditorGUILayout.HelpBox("This object timeline is currently MAIN.", MessageType.Info);
            if (GUILayout.Button("Set the other timeline object to MAIN"))
            {
                objBaseScript.IsMain = false;
                objBaseScript.IsVisible = false;
                objBaseScript.otherTimelineRef.IsMain = true;
                objBaseScript.otherTimelineRef.IsVisible = true;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("This object timeline is NOT MAIN", MessageType.Error);
            if (GUILayout.Button("Set this timeline object to MAIN"))
            {
                objBaseScript.IsMain = true;
                objBaseScript.IsVisible = true;
                objBaseScript.otherTimelineRef.IsMain = false;
                objBaseScript.otherTimelineRef.IsVisible = false;
            }
        }

        EditorGUILayout.Space();

        objBaseScript.ItemType = (TimelineObject)EditorGUILayout.EnumPopup("Item Type: ", objBaseScript.ItemType);
        objBaseScript.IsVisible = EditorGUILayout.Toggle("Is Visible: ", objBaseScript.IsVisible);

        if (objBaseScript.ItemType.Equals(TimelineObject.Present))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            objBaseScript.PresentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", objBaseScript.PresentMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        else if (objBaseScript.ItemType.Equals(TimelineObject.Past))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            objBaseScript.PastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", objBaseScript.PastMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }

#pragma warning disable CS0618 // Type or member is obsolete
        objBaseScript.otherTimelineRef = (StateObject)EditorGUILayout.ObjectField("Other Timeline Reference", objBaseScript.otherTimelineRef, typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete

        EditorGUILayout.Space();

        objBaseScript.ThrowableDistance = EditorGUILayout.IntField("Throwable Distance", objBaseScript.ThrowableDistance);
        objBaseScript.movementVelocity = EditorGUILayout.FloatField("Movement", objBaseScript.movementVelocity);


        //base.OnInspectorGUI();
        */
    }

}
