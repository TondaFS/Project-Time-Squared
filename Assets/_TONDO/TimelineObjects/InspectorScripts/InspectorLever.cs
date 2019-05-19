using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorLever : MonoBehaviour {
    public bool areTargetsCommon = true;
    public List<GameObject> commonTargets;
    public float leverChangeVelocity = 5f;

    [Header("PRESENT")]
    public Material presentMaterial;
    [Tooltip("Jake vsechny objekty, majici nejaky stav, v pritomnosti ovlivnuje.")]
    public List<GameObject> presentTargets;
    public bool isPresentActive;

    [Header("PAST")]
    public Material pastMaterial;
    [Tooltip("Jake vsechny objekty, majici nejaky stav, v minulosti ovlivnuje.")]
    public List<GameObject> pastTargets;
    public bool isPastActive;

    protected virtual void Awake()
    {
        if (transform.position.Equals(new Vector3(100, -100, -100)))
            return;

        GameObject parent = new GameObject();
        parent.transform.position = transform.position;
        parent.name = "Lever";

        GameObject pres = this.gameObject;
        pres.transform.parent = parent.transform;
        pres.transform.localPosition = new Vector3(0, 0, 0);
        pres.name = "PRESENT";

        GameObject past = Instantiate(pres, new Vector3(100, -100, -100), transform.rotation);
        past.name = "PAST";
        past.transform.parent = parent.transform;
        past.transform.localPosition = new Vector3(0, 0, 0);

        Destroy(past.GetComponent<InspectorLever>());

        if (areTargetsCommon)
        {
            pres.AddComponent<Lever>().CreateLever(TimelineObject.Present, presentMaterial,
                commonTargets, isPresentActive, leverChangeVelocity);
            past.AddComponent<Lever>().CreateLever(TimelineObject.Past, pastMaterial,
                commonTargets, isPastActive, leverChangeVelocity);
        }
        else
        {
            pres.AddComponent<Lever>().CreateLever(TimelineObject.Present, presentMaterial,
                presentTargets, isPresentActive, leverChangeVelocity);
            past.AddComponent<Lever>().CreateLever(TimelineObject.Past, pastMaterial,
                pastTargets, isPastActive, leverChangeVelocity);
        }

        pres.GetComponent<StateObject>().otherTimelineRef = past.GetComponent<StateObject>();
        past.GetComponent<StateObject>().otherTimelineRef = pres.GetComponent<StateObject>();

        Destroy(this);
    }
}
