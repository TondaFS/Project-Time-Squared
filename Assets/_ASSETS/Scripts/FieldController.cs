using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class FieldController : MonoBehaviour {
    private float radius = 10;
    private float realFuckingRadius = 0;
    public float Radius
    {
        get
        {
            return radius;
        }

        set
        {
            if (value == 0)
            {
                radius = 0;
                realFuckingRadius = 0;
            }
            if(value < 1)
            {
                radius = value;
                realFuckingRadius = 1.49f * value;
            }
            else
            {
                radius = value;
                realFuckingRadius = radius + 0.49f;
            }                
        }
    }

    /// <summary>
    /// On awake it sets border global width and color for all shaders to use. Also reset pos and radius.
    /// </summary>
    void Awake()
    {
        Shader.SetGlobalFloat("_BorderWidth", 1);
        Shader.SetGlobalColor("_BorderColor", new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
        Shader.SetGlobalVector("_FieldPos", new Vector3(-100, -100, -100));
        Shader.SetGlobalFloat("_FieldRad", 0);
    }

    /// <summary>
    /// On update it sets global position and radius of the field for all shaders to use.
    /// </summary>
    void Update () 
    {
        int c = 2;
        transform.localScale = new Vector3(realFuckingRadius * c, realFuckingRadius * c , realFuckingRadius * c);
        transform.eulerAngles = new Vector3(0,0,0);

        Shader.SetGlobalVector("_FieldPos", transform.position);
        Shader.SetGlobalFloat("_FieldRad", realFuckingRadius);
    }
}
