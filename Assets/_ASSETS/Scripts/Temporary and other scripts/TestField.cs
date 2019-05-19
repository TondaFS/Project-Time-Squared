using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script, který se používá pro testování efektů časového pole v editoru nebo za run timu bez potřeby použít generátor. 
/// Ovlivňuje pouze shadery, ne kolize.
/// </summary>
[ExecuteInEditMode]
public class TestField : MonoBehaviour {
    public float radius = 10;
  
    void Update () 
    {
        transform.localScale = new Vector3(radius*2, radius*2 , radius*2);

        Shader.SetGlobalVector("_FieldPos", transform.position);
        Shader.SetGlobalFloat("_FieldRad", radius);
    }
}
