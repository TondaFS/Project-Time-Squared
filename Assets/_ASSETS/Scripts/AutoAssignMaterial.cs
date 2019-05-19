using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAssignMaterial : MonoBehaviour {

	public Material material;
	void Start()
     {
        MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.material = material;

        DestroyImmediate(this);
     }
}
