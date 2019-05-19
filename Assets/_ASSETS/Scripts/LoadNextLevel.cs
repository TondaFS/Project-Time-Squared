using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour {

    public int nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10) //10: Player
        {

            if (nextScene != 0) {
                ProgressManager.instance.levels[nextScene - 1].isUnlocked = true;
                ProgressManager.instance.Save();
            }
            
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        
    }
}
