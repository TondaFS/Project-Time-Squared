using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Project Time Squared/Generators/LaserReceiverInsp")]
public class LaserReceiverInspector : MonoBehaviour {
    public bool occupyTile;

    public TimelineObject timeline;
    public Material presentMaterial;
    [Tooltip("Jake vsechny objekty, majici nejaky stav, v pritomnosti ovlivnuje.")]
    public List<GameObject> targets;
    public bool isPresentActive;
    public GameObject alternativeMeshPresent;
    
    protected virtual void Awake()
    {
        if (transform.position.Equals(new Vector3(100, -100, -100)))
            return;
        
        this.name = "Laser Receiver";
   
        
        this.gameObject.AddComponent<LaserReceiver>().CreateReceiver(timeline, presentMaterial,
                targets, isPresentActive, occupyTile);
    
        Destroy(this);
    }
}
