using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Activator {
    public float leverChangeVelocity = 5f;
    Vector3 activeRot;
    Vector3 inactiveRot;

    Coroutine stateChangeCor;

    protected override void Start()
    {
        
        stateChangeCor = null;

        base.Start();

        activeRot = new Vector3(15, transform.rotation.y, 0);
        inactiveRot = new Vector3(-10, transform.rotation.y, 0);
        
        StartCoroutine(ChangeLeverState());
                
        //transform.localRotation = notActiveRotation;
        
    }

    public override void OnActivate()
    {
        base.OnActivate();

        if (stateChangeCor != null)
        {
            Debug.Log("Cor is not null");
            StopCoroutine(stateChangeCor);
        }
            
        stateChangeCor = StartCoroutine(ChangeLeverState());
    }

    public override void Activate()
    {
        StartCoroutine(WaitForItemsInit());
    }

    public void CreateLever(TimelineObject type, Material mat, List<GameObject> targets, bool isActive, float vel)
    {
        ItemType = type;
        material = mat;
        goTargets = targets;
        IsActivated = isActive;
        OccupyTile = true;
        leverChangeVelocity = vel;
    }
    
    IEnumerator ChangeLeverState()
    {
        bool moving = true;
        Debug.Log("Changign state");
                
        while (moving)
        {
            //Debug.Log("Moving");
            if (IsActivated)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(activeRot),
                    leverChangeVelocity * Time.deltaTime);

                if (transform.rotation == (Quaternion.Euler(activeRot)))
                    moving = false;
                else
                    yield return null;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(inactiveRot),
                    leverChangeVelocity * Time.deltaTime);

                if (transform.rotation == (Quaternion.Euler(inactiveRot)))
                    moving = false;
                else
                    yield return null;
            }
        }
        //Debug.Log("We are done");
    }




}
