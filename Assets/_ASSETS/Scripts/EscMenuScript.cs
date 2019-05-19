using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;

public class EscMenuScript : MonoBehaviour {

    ThirdPersonCharacter tpc;
    public GameObject canvas;
    bool isTrue;

	void Awake () {
        tpc = GetComponent<ThirdPersonCharacter>();
        isTrue = false;
	}
	
	void Update () {
        if (Input.GetButtonDown("Escape"))
        {
            canvas.SetActive(!isTrue);
            tpc.enabled = isTrue;
            isTrue = !isTrue;
        }
	}

    public void ContinueButton()
    {
        tpc.enabled = true;
        canvas.SetActive(false);
        isTrue = false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
