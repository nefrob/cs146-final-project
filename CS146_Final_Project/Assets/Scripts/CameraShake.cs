/*
* File:        Camera Shake
* Author:      ftvs: https://gist.github.com/ftvs/5822103#file-camerashake-cs
*              Robert Neff
* Date:        11/09/17
* Description: Shakes camera for provided duration on enable.
*/

using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Camera transfrom to shake
	public Transform camTransform;	
	// Shake time
	public float shakeDuration = 0f;
	// Shake amplitude
	public float shakeAmount = 0.7f;
    public float shakeDepletionRate = 1.0f;
	
    // Starting camera position
	Vector3 startPos;
	
    /* Get the camera transform. */
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
    /* Set position to revert to. */
	void OnEnable()
	{
        startPos = camTransform.localPosition;
	}

    /* Shake camera while active and duration greater than zero. */
	void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = startPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= shakeDepletionRate * Time.deltaTime;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = startPos;
		}
	}
}
