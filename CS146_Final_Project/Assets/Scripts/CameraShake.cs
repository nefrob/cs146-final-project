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
	public Transform cameraTransform;	
	// Shake time
	public float shakeDuration = 0f;
	// Shake amplitude
	public float shakeAmount = 0.7f;
    public float shakeDepletionRate = 1.0f;
	
    // Starting camera position
	private Vector3 startPos;
	
    /* Set position to revert to. */
	void OnEnable()
	{
        startPos = cameraTransform.localPosition;
	}

    /* Shake camera while active and duration greater than zero. */
	void Update()
	{
		if (shakeDuration > 0)
		{
            cameraTransform.localPosition = startPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= shakeDepletionRate * Time.deltaTime;
		}
		else
		{
			shakeDuration = 0f;
            cameraTransform.localPosition = startPos;
		}
	}
}
