using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIBaseManage : FadeInOut {

    private Image fadeImage;
    private GameObject fadeImageObject;

    public static UIBaseManage UIInstance;

    private void Awake()
    {
        if (UIInstance == null)
        {
            UIInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);     
        }

        StartCoroutine(FadeImageSet());
    }

    private IEnumerator FadeImageSet()
    {
        fadeImageObject = GameObject.FindGameObjectWithTag("Fadescreen");
        fadeImage = fadeImageObject.GetComponent<Image>();
        yield return null;
        if (fadeImageObject == null && fadeImage == null)
        {
            fadeImageObject = Instantiate(Resources.Load("Prefab/ETC/FadeImage"), GameObject.FindGameObjectWithTag("UI").transform) as GameObject;
            fadeImage = fadeImageObject.GetComponent<Image>();
            fadeImageObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
		try
		{
			StartCoroutine(FadeIn(fadeImage));
		}
		catch
		{
			StartCoroutine(FadeImageSet());
		}
        yield return new WaitForFixedUpdate();
        fadeImageObject.transform.SetAsLastSibling();
    }

    public IEnumerator LoadSceneAndFadeInOut(string name)
    {
        AudioManager.Instance.PlayOneShotEffectClipByName("BeforeSceneChanged");
        IEnumerator FO = FadeOut(fadeImage);
        StartCoroutine(FO);
        yield return new WaitUntil(() => FO.MoveNext() == false);
        fadeImage = null;
        fadeImageObject = null;

        yield return new WaitForSeconds(1f);

        IEnumerator SM = SceneManage(name);
        StartCoroutine(SM);
        yield return new WaitUntil(() => SM.MoveNext() == false);
        AudioManager.Instance.PlayOneShotEffectClipByName("AfterSceneChanged");
        StartCoroutine(FadeImageSet());
    }

    private IEnumerator SceneManage(string name)
    {
        SceneManager.LoadScene(name);
        yield return null;
    }
}
