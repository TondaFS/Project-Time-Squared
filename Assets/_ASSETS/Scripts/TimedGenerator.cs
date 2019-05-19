using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGenerator : MonoBehaviour 
{
    public float toggleTime;
    private Generator generator;

    private IEnumerator coroutine;

    private void Start()
    {
        generator = GetComponent<Generator>();

        coroutine = ToggleGenerator(toggleTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator ToggleGenerator(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            generator.Activate();
        }
    }

}
