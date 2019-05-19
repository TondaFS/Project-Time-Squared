using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Project Time Squared/Generators/InspectorGeneratorStatic")]
public class InspectorGeneratorStatic : MonoBehaviour {
    public TimelineObject itemType;

    public Material material;
    public bool isOn;
    public int areaOfEffect;

    public float expansionTime;
    public bool isAlwaysOn = false;

    private void Awake()
    {
        gameObject.AddComponent<Generator>().CreateGenerator(itemType, material, isOn, areaOfEffect, false, false, expansionTime, isAlwaysOn);
        gameObject.name = "StaticGenerator";
        Destroy(this);
    }
}
