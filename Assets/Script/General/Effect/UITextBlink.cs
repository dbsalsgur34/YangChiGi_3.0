using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class UITextBlink : MonoBehaviour {
    private Text MatchingMessage;
    // Use this for initialization

    public Color blinkColor;
    [Range(0f,0.2f)]
    public float blinkSpeed;
	void Awake () {
        MatchingMessage = this.GetComponent<Text>();
	}

    private void OnEnable()
    {
        StartCoroutine(TextBlink());
    }

    private IEnumerator TextBlink()
    {
        float i;
        while (true)
        {
            for (i = 1f; i >= 0; i -= blinkSpeed)
            {
                blinkColor.a = i;
                MatchingMessage.color = blinkColor;
                yield return null;
            }

            for (i = 0f; i <= 1; i += blinkSpeed)
            {
                blinkColor.a = i;
                MatchingMessage.color = blinkColor;
                yield return null;
            }
        }
    }
}
