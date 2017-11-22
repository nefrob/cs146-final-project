using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeOutSpeed;

    private int drawDepth = 1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

	// Use this for initialization
	void OnGui () {

        alpha += fadeDir * fadeOutSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01 (alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

    public float BeginFade(int direction) {
        fadeDir = direction;
        return (fadeOutSpeed);
    }

    private void OnLevelWasLoaded(int level)
    {
        BeginFade(-1);
    }
}
