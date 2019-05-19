using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorDoor : MonoBehaviour {
    [Header("PRESENT DOOR")]
    public bool presentIsOpen;
    public DoorOpening presentOpeningMode;
    public float presentOpeningVelocity = 5;
    public Material presentMat;
    public bool presentNeedAllButtons;

    [Header("PAST DOOR")]
    public bool pastIsOpen;
    public DoorOpening pastOpeningMode;
    public float pastOpeninVelocity = 5;
    public Material pastMaterial;
    public bool pastNeedAllButtons;

    protected virtual void Awake()
    {
        if (transform.position.Equals(new Vector3(100, -100, -100)))
            return;

        GameObject parent = new GameObject();
        parent.transform.position = transform.position;
        parent.name = "Door";

        GameObject pres = this.gameObject;
        pres.transform.parent = parent.transform;
        pres.transform.localPosition = new Vector3(0, 0, 0);
        pres.name = "PRESENT";

        GameObject past = Instantiate(pres, new Vector3(100, -100, -100), transform.rotation);
        past.name = "PAST";
        past.transform.parent = parent.transform;
        past.transform.localPosition = new Vector3(0, 0, 0);

        Destroy(past.GetComponent<InspectorDoor>());

        pres.AddComponent<Door>().CreateDoor(TimelineObject.Present, presentIsOpen, presentOpeningMode, presentMat, presentOpeningVelocity, presentNeedAllButtons);
        past.AddComponent<Door>().CreateDoor(TimelineObject.Past, pastIsOpen, pastOpeningMode, pastMaterial, pastOpeninVelocity, pastNeedAllButtons);

        pres.GetComponent<StateObject>().otherTimelineRef = past.GetComponent<StateObject>();
        past.GetComponent<StateObject>().otherTimelineRef = pres.GetComponent<StateObject>();

        Destroy(this);
    }
}
