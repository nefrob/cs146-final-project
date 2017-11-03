using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingUI : MonoBehaviour {
    // Test to make falsh
    public Text flashingText;
    public string startText = "";
    public bool changeText = false;
    // Speed of flash
    public float flashSpeed = 0.5f;
    // Colors to change to/from
    public Color first = Color.white;
    public Color second = Color.magenta;
    public bool useColor = false;

    /* Loop to make text flash. */
    public IEnumerator MakeTextFlash()
    {
        while (true)
        {
            if (useColor) flashingText.color = first;
            if (changeText) flashingText.text = "";
            yield return new WaitForSeconds(flashSpeed);
            if (useColor) flashingText.color = second;
            if (changeText) flashingText.text = startText;
            yield return new WaitForSeconds(flashSpeed);
            // Break if no longer active
            if (!gameObject.activeInHierarchy) break;
        }
    }

    /* Restart coroutine on enable. */
    void OnEnable()
    {
        StartCoroutine(MakeTextFlash());
    }
}

