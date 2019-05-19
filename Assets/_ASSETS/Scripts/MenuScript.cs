using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public GameObject main;
    public GameObject Levels;
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void GoBack()
    {
        Levels.SetActive(false);
        main.SetActive(true);
    }

    public void ShowLevels()
    {
        Levels.SetActive(true);
        main.SetActive(false);
    }

}
