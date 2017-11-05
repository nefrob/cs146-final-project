/*
* File:        Camera OVerlay
* Author:      Robert Neff
* Date:        10/28/17
* Description: Overlays an effect material on the camera.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraOverlay : MonoBehaviour {
    // Material to apply to the camera
    [SerializeField] private Material crtEffectMaterial;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Color otherColor;
    [SerializeField] private bool useOtherColor;
    private Color startColor;

    /* Set start color for reference. */
    void Start()
    {
        if (useOtherColor) startColor = otherColor;
        else startColor = crtEffectMaterial.color;
    }

    /* Render texture to the screen. */
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Color temp = startColor;
        temp.r = (1 - controller.shieldBarSlider.value) * temp.r;
        temp.b = (1 - controller.shieldBarSlider.value) * temp.b;
        temp.g = (1 - controller.shieldBarSlider.value) * temp.g;
        crtEffectMaterial.color = temp;
        Graphics.Blit(source, destination, crtEffectMaterial);
    }
}
