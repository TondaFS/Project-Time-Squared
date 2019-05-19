using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller, který hýbe s kamerou a rotuje s ní kolem nějakého bodu (zejména hráče).
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	/// <summary>
	/// Bod, se kterým se kamera hýbe a kolem kterého se rotuje. 
	/// </summary>
	public Transform followPoint;
	/// <summary>
	/// Pozice kamery relativně k pozici followPointu. 
	/// </summary>
	public Vector3 cameraPosition = new Vector3(10.5f, 8, 0);
	/// <summary>
	/// Natočení kamery.
	/// </summary>
	public Vector2 cameraRotation = new Vector2(30,0);

	private Coroutine rotationCoroutine = null;
	private Boolean blockRotateBtn = false;
	private Boolean rotatedLeft = false;
	
	void Start()
	{
		UpdateCameraRotation();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
	

		if (!blockRotateBtn && Input.GetButtonDown("Rotate Camera")){
			if (!rotatedLeft)
			{
				if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
				rotationCoroutine = StartCoroutine(RotateCamera(-90, 0.1f, true));	
			}
			else
			{
				if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);	
				rotationCoroutine = StartCoroutine(RotateCamera(0, 0.1f, false));
			}			
			rotatedLeft = !rotatedLeft;
		}		
		UpdateCameraPosition();		
	}

	/// <summary>
	/// Korutina, která orotuje kameru kolem followPointu.
	/// </summary>
    private IEnumerator RotateCamera(float endRotation, float rotationLength, bool directionIsLeft)
    {
		float startRotation = cameraRotation.y;
		float currentTime = 0;


		if(directionIsLeft) {

			while(cameraRotation.y >= endRotation) {
				currentTime += Time.deltaTime;
				
				float t = currentTime;
				t = 1f - Mathf.Cos((t * Mathf.PI) / (rotationLength * 2));

				cameraRotation.y = Mathf.Lerp(startRotation, endRotation-0.1f, t);
				
				UpdateCameraRotation();	
				UpdateCameraPosition();
				yield return null;				
			}				
		}
    	else{

			while(cameraRotation.y <= endRotation) {
				currentTime += Time.deltaTime;
				
				float t = currentTime;
				t = 1f - Mathf.Cos((t * Mathf.PI) / (rotationLength * 2));

				cameraRotation.y = Mathf.Lerp(startRotation, endRotation+0.1f, t);
				
				UpdateCameraRotation();	
				UpdateCameraPosition();
				yield return null;				
			}

		}
		cameraRotation.y = endRotation;
		UpdateCameraRotation();
		rotationCoroutine = null;
    }

	/// <summary>
	/// Aktualizuje transform.position kamery podle hodnot ve skriptu.
	/// </summary>
    private void UpdateCameraPosition()
    {
        float cameraRotationRad = cameraRotation.y * Mathf.Deg2Rad;
		float cameraPosX = cameraPosition.x * Mathf.Cos(cameraRotationRad) - cameraPosition.z * Mathf.Sin(cameraRotationRad);
		float cameraPosZ = cameraPosition.z * Mathf.Cos(cameraRotationRad) + cameraPosition.x * Mathf.Sin(cameraRotationRad);

		Vector3 newCameraPosition;
		if(followPoint == null) newCameraPosition = new Vector3(cameraPosX, cameraPosition.y, cameraPosZ);
		else newCameraPosition = new Vector3(cameraPosX + followPoint.position.x, cameraPosition.y, cameraPosZ + followPoint.position.z);
		
		transform.position = newCameraPosition;		
    }

	/// <summary>
	/// Aktualizuje transform.rotation kamery podle hodnot ve skriptu.
	/// </summary>
    private void UpdateCameraRotation()
    {
		cameraRotation.y %= -360;
		Vector3 newCameraRotation = new Vector3(cameraRotation.x, -(cameraRotation.y + 90), transform.eulerAngles.z);
		transform.rotation = Quaternion.Euler(newCameraRotation);
    }
}
