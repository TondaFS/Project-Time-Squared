using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Project Time Squared/Generators/InspectorStaticGeometry")]
public class InspectorStaticGeometry : MonoBehaviour {
    public TimelineObject ItemType;
    public Material material;

    private void Awake()
    {
        gameObject.AddComponent<StaticGeometry>().CreateStaticGeometry(ItemType, material);
        gameObject.name = "Static Geometry";
        Destroy(this);
    }
}
