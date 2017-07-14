using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageWindow : MonoBehaviour {

    private Text ErrorMessage;

    public void InitErrorMessageWindow()
    {
        ErrorMessage = GetComponentInChildren<Text>();
        this.gameObject.SetActive(false);
    }

    public IEnumerator ShowMessage(string targetMessage)
    {
        int messageLength = targetMessage.Length;
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * messageLength, 100);
        this.ErrorMessage.text = targetMessage;
        yield return new WaitUntil(() => Input.anyKeyDown);
        this.gameObject.SetActive(false);
    }
}
