using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class KarelAnimatorController : MonoBehaviour {

    Animator animator;
    RobotController karelController;
    ThirdPersonUserControl controls;
    ThirdPersonCharacter charControl;

    [SerializeField] Transform handsReference;
    MovableObject item;
    Vector3 dropPosition;
    Quaternion dropRotation;
    Transform dropParent;
    bool preassureParent;
       

    private void Start()
    {
        animator = GetComponent<Animator>();
        karelController = transform.root.GetComponent<RobotController>();
        controls = transform.root.GetComponent<ThirdPersonUserControl>();
        charControl = transform.root.GetComponent<ThirdPersonCharacter>();
    }

    public void StartPickup()
    {
        charControl.GetComponent<Rigidbody>().Sleep();
        SetControls(false);
        animator.SetTrigger("PickUP");        
    }

    public void SetPickupObject() {
        item = karelController.CarriingItem;
        item.transform.parent = handsReference;
        karelController.CheckPreassurePlateOnTIle();

        //tests
        /*
        item.transform.localPosition = new Vector3(0, 0, 0);

        Vector3 v = handsReference.position - item.GetComponent<Renderer>().bounds.center;
        item.transform.localPosition -= v;
        */
    }

    public void SetDrop()
    {
        if (item == null)
        {
            Debug.Log("KAREL ANIMATOR: Item is null to drop!");
            return;
        }

        item.transform.parent = dropParent;
        item.transform.localPosition = dropPosition;
        item.transform.rotation = dropRotation;

        if (preassureParent)
            karelController.CheckPreassurePlateOnTIle();
    }

    void SetControls(bool value)
    {
        controls.enabled = value;
        charControl.enabled = value;
    }

    public void StartDrop(Vector3 pos, Quaternion rot, Transform parent, bool checkPP)
    {
        charControl.GetComponent<Rigidbody>().Sleep();
        SetControls(false);
        animator.SetTrigger("Drop");
        dropParent = parent;
        dropPosition = pos;
        dropRotation = rot;
        preassureParent = checkPP;
    }

    void EnableControls()
    {
        charControl.GetComponent<Rigidbody>().WakeUp();
        SetControls(true);
    }
}
