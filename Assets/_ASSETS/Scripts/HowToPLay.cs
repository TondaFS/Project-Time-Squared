using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class HowToPLay : MonoBehaviour {
    public EscMenuScript ems;
    
    private void Start()
    {
        ems.enabled = false;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            Continue();
    }

    public void Continue()
    {
        ems.enabled = true;
        this.gameObject.SetActive(false);        
    }
}
