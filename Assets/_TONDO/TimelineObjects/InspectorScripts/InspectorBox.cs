using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Project Time Squared/Items/PTS_Box")]
public class InspectorBox : MonoBehaviour {
    public TimelineObject itemType;
    public Material material;

    private void Awake()
    {
        gameObject.AddComponent<Box>().CreateBox(itemType, material);
        gameObject.name = "Box";
        Destroy(this);
    }
}
