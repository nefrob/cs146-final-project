using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraOverlay : MonoBehaviour {
    // Material to apply to the camera
    [SerializeField] private Material EffectMaterial;

    /* Render texture to the screen. */
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, EffectMaterial);
    }
}
