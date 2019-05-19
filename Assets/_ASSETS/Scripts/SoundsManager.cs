using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour {
    public static SoundsManager instance;

    public List<AudioClip> drop;
    public AudioClip buttonPressed;
    public AudioClip generator;

    AudioSource source;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayDrop() {
        int i = Random.Range(0, drop.Count);
        source.PlayOneShot(drop[i]);
    }

    public void PlayButtonPressed()
    {
        if (source == null)
            return;

        source.PlayOneShot(buttonPressed);
    }


}
