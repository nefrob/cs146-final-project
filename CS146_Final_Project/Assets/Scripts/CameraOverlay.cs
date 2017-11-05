/*
* File:        Camera Overlay
* Author:      Robert Neff
* Date:        11/05/17
* Description: Overlays an effect material on the camera that scales with
*              player shield value.
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
    // Powers
    [Range(0.0f, 1.0f)] public float baseColorPower = 0.0f; // 0-1
    [Range(0.0f, 1.0f)] public float fullColorPower = 1.0f; // no greater than 1 - baseColorPower

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
        // Compute amount to change material color by
        temp.r = baseColorPower * temp.r + fullColorPower * (1 - controller.shieldBarSlider.value) * temp.r;
        temp.b = baseColorPower * temp.b + fullColorPower * (1 - controller.shieldBarSlider.value) * temp.b;
        temp.g = baseColorPower * temp.g + fullColorPower * (1 - controller.shieldBarSlider.value) * temp.g;
        crtEffectMaterial.color = temp;
        Graphics.Blit(source, destination, crtEffectMaterial);
    }
}
