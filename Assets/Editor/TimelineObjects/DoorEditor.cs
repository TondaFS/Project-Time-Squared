using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {

    public override void OnInspectorGUI()
    {
        Door objBaseScript = (Door)target;
        DrawDefaultInspector();
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
        }

        //base.OnInspectorGUI();

        objBaseScript.ItemType = (TimelineObject)EditorGUILayout.EnumPopup("Item Type: ", objBaseScript.ItemType);
        objBaseScript.IsVisible = EditorGUILayout.Toggle("Is Visible: ", objBaseScript.IsVisible);

        if (objBaseScript.ItemType.Equals(TimelineObject.Present))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //objBaseScript.PresentMaterial = (Material)EditorGUILayout.ObjectField("Present Material", objBaseScript.PresentMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }
        else if (objBaseScript.ItemType.Equals(TimelineObject.Past))
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //objBaseScript.PastMaterial = (Material)EditorGUILayout.ObjectField("Past Material", objBaseScript.PastMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete
        }

#pragma warning disable CS0618 // Type or member is obsolete
        objBaseScript.otherTimelineRef = (StateObject)EditorGUILayout.ObjectField("Other Timeline Reference", objBaseScript.otherTimelineRef, typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete

        EditorGUILayout.Space();
        objBaseScript.OpeningMode = (DoorOpening)EditorGUILayout.EnumPopup("Opening Mode:", objBaseScript.OpeningMode);

        if (!objBaseScript.IsOpen)
        {
            EditorGUILayout.HelpBox("Doors are Closed.", MessageType.Info);
            if (GUILayout.Button("OPEN DOOR"))
            {
                objBaseScript.IsOpen = true;
                objBaseScript.SetDoorPos(objBaseScript.OpeningMode);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Doors are Opened", MessageType.Info);
            if (GUILayout.Button("CLOSE DOOR"))
            {
                objBaseScript.IsOpen = false;
                objBaseScript.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        
        if (GUILayout.Button("RESET OPEN POSTION"))
        {
            objBaseScript.SetDoorPos(objBaseScript.OpeningMode);
        }
        */
    }
}
