using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Project Time Squared/Generators/InspectorPreassurePlate")]
public class InspectorPreassurePlate : MonoBehaviour {
    public bool areTargetsCommon = true;
    public List<GameObject> commonTargets;

    [Header("PRESENT")]
    public Material presentMaterial;
    [Tooltip("Jake vsechny objekty, majici nejaky stav, v pritomnosti ovlivnuje.")]
    public List<GameObject> presentTargets;
    public GameObject alternativeMeshPresent;

    [Header("PAST")]
    public Material pastMaterial;
    [Tooltip("Jake vsechny objekty, majici nejaky stav, v minulosti ovlivnuje.")]
    public List<GameObject> pastTargets;
    public GameObject alternativeMeshPast;

    protected virtual void Awake()
    {
        if (transform.position.Equals(new Vector3(100, -100, -100)))
            return;

        GameObject parent = new GameObject();
        parent.transform.position = transform.position;
        parent.name = "Preassure Plate";

        GameObject pres;
        bool removeThis = false;

        if (alternativeMeshPresent != null)
        {
            pres = Instantiate(alternativeMeshPresent);
            removeThis = true;
        }
        else
            pres = this.gameObject;

        pres.transform.parent = parent.transform;
        pres.transform.localPosition = new Vector3(0, 0, 0);
        pres.name = "PRESENT";

        GameObject past;

        if (alternativeMeshPast != null)
            past = Instantiate(alternativeMeshPast, new Vector3(100, -100, -100), transform.rotation);
        else
            past = Instantiate(pres, new Vector3(100, -100, -100), transform.rotation);

        past.name = "PAST";
        past.transform.parent = parent.transform;
        past.transform.localPosition = new Vector3(0, 0, 0);

        Destroy(past.GetComponent<InspectorPreassurePlate>());
        
        if (areTargetsCommon)
        {
            pres.AddComponent<PreassurePlate>().CreatePreassurePlate(TimelineObject.Present, presentMaterial,
                commonTargets);
            past.AddComponent<PreassurePlate>().CreatePreassurePlate(TimelineObject.Past, pastMaterial,
                commonTargets);
        }
        else
        {
            pres.AddComponent<PreassurePlate>().CreatePreassurePlate(TimelineObject.Present, presentMaterial,
                presentTargets);
            past.AddComponent<PreassurePlate>().CreatePreassurePlate(TimelineObject.Past, pastMaterial,
                pastTargets);
        }

        pres.GetComponent<StateObject>().otherTimelineRef = past.GetComponent<StateObject>();
        past.GetComponent<StateObject>().otherTimelineRef = pres.GetComponent<StateObject>();

        if (removeThis)
            Destroy(this.gameObject);

        Destroy(this);
    }
}
