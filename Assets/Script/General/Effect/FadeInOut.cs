using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public IEnumerator FadeIn(Image targetImage)
    {
        targetImage.raycastTarget = true;
        for (float i = 1f; i >= 0; i -= 0.025f)
        {
            Color color = new Vector4(0, 0, 0, i);
            targetImage.color = color;
            yield return 0;
        }
        targetImage.raycastTarget = false;
    }

    public IEnumerator FadeOut(Image targetImage)
    {
        targetImage.raycastTarget = true;
        for (float i = 0f; i <= 1; i += 0.025f)
        {
            Color color = new Vector4(0, 0, 0, i);
            targetImage.color = color;
            yield return 0;
        }
        //targetImage.gameObject.SetActive(false);
    }
}
