using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateObject))]
public class ObjectBaseEditor : Editor {
    private GUILayoutOption[] allowSceneObjects;

    public override void OnInspectorGUI()
    {
        StateObject objBaseScript = (StateObject)target;

        /*
        if (objBaseScript.IsMain)
        {
            EditorGUILayout.HelpBox("This object timeline is currently MAIN.", MessageType.Info);
            if (GUILayout.Button("Set the other timeline object to MAIN"))
            {
                objBaseScript.IsMain = false;
                objBaseScript.otherTimelineRef.IsMain = true;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("This object timeline is NOT MAIN", MessageType.Error);
            if (GUILayout.Button("Set this timeline object to MAIN"))
            {
                objBaseScript.IsMain = true;
                objBaseScript.otherTimelineRef.IsMain = false;
            }

        }*/

        EditorGUILayout.Space();
        objBaseScript.ItemType = (TimelineObject)EditorGUILayout.EnumPopup("Item Type: ", objBaseScript.ItemType);
        objBaseScript.IsVisible = EditorGUILayout.Toggle("Is Visible: ", objBaseScript.IsVisible);

        if (objBaseScript.ItemType.Equals(TimelineObject.Present))
        {

#pragma warning disable CS0618 // Type or member is obsolete
            //objBaseScript.PresentMaterial = (Material)EditorGUILayout.ObjectField("PresentMaterial", objBaseScript.PresentMaterial, typeof (Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        else if (objBaseScript.ItemType.Equals(TimelineObject.Past))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //objBaseScript.PastMaterial = (Material)EditorGUILayout.ObjectField("PastMaterial", objBaseScript.PresentMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        //base.OnInspectorGUI();
    }    
}
