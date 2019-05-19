using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour {
    public UnityEngine.UI.Button[] Levels;

    private void OnEnable()
    {
        for(int i = 0; i < ProgressManager.instance.levels.Length; i++)
        {
            if (ProgressManager.instance.levels[i].isUnlocked)
            {
                Levels[i].interactable = true;
            } else
                Levels[i].interactable = false;
        }
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
}
