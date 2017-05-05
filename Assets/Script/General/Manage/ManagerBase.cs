using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerBase : FadeInOut {

    public Image FadeImage = null;

    public virtual void Awake()
    {
        StartCoroutine("FadeImageSet");
    }

    protected IEnumerator FadeImageSet()
    {
        GameObject FadeImageObject = null;
        if (FadeImageObject == null)
        {
            FadeImageObject = Instantiate(Resources.Load("Prefab/ETC/FadeImage"), GameObject.FindGameObjectWithTag("UI").transform) as GameObject;
        }
        FadeImageObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        FadeImage = FadeImageObject.GetComponent<Image>();
        yield return null;
        StartCoroutine(StartRoutine());
    }

    protected IEnumerator StartRoutine()
    {
        PlayManage.Instance.SetFadeImage(FadeImage);
        yield return new WaitUntil(()=>PlayManage.Instance.FadeImage.Equals(this.FadeImage) == true);
        IEnumerator FI = FadeIn(FadeImage);
        StartCoroutine(FI);
    }

    public virtual void Start()
    {

    }
}
