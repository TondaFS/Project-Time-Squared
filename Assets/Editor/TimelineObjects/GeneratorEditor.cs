using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor {
    /*
    public override void OnInspectorGUI()
    {       
        Generator g = (Generator)target;

        if(GUILayout.Button("Toggle generator"))
        {
            g.Activate();
        }

#pragma warning disable CS0618 // Type or member is obsolete
        //g.PresentMaterial = (Material)EditorGUILayout.ObjectField("Material", g.PresentMaterial, typeof(Material));
#pragma warning restore CS0618 // Type or member is obsolete

        g.IsOn = EditorGUILayout.Toggle("Is On", g.IsOn);
        g.AreaOfEffect = EditorGUILayout.IntField("Area of Effect", g.AreaOfEffect);
        g.affectsTheTile = EditorGUILayout.Toggle("Affects the Tile", g.affectsTheTile);
        
        //base.OnInspectorGUI();
    }
    */

}
