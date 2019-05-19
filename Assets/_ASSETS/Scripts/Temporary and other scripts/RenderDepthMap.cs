using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderDepthMap : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }
}