/*
* File:        Camera Overlay
* Author:      Robert Neff
* Date:        11/09/17
* Description: Overlays an effect material on the camera that scales with
*              player shield value.
*/

using UnityEngine;

//[ExecuteInEditMode]
public class CameraOverlay : MonoBehaviour {
    // Material to apply to the camera
    [SerializeField] private Material crtEffectMaterial;
    private UIHandler uiScript;
    [SerializeField] private Color otherColor;
    [SerializeField] private bool editColorOnCamera;
    private Color startColor;
    // Powers
    [Range(0.0f, 1.0f)] public float baseColorPower = 0.0f; // 0-1
    [Range(0.0f, 1.0f)] public float fullColorPower = 1.0f; // no greater than 1 - baseColorPower

    /* Set start color for reference. */
    void Start()
    {
        uiScript = FindObjectOfType<UIHandler>();

        if (editColorOnCamera) startColor = otherColor;
        else startColor = crtEffectMaterial.color;
    }

    /* Allow runtime shader color changes for debugging. */
    void Update()
    {
        if (editColorOnCamera) startColor = otherColor;
    }

    /* Render texture to the screen. */
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Color temp = startColor;
        // Compute amount to change material color by
        temp.r = baseColorPower * temp.r + fullColorPower * (1 - uiScript.shieldBarSlider.value) * temp.r;
        temp.b = baseColorPower * temp.b + fullColorPower * (1 - uiScript.shieldBarSlider.value) * temp.b;
        temp.g = baseColorPower * temp.g + fullColorPower * (1 - uiScript.shieldBarSlider.value) * temp.g;
        crtEffectMaterial.color = temp;
        Graphics.Blit(source, destination, crtEffectMaterial);
    }
}
