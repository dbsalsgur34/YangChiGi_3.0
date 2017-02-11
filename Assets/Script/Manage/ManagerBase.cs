using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase : FadeInOut {

    public virtual void Awake()
    {
        PlayManage.Instance.SearchFadeImage();
        StartCoroutine(PlayManage.Instance.FadeIn(PlayManage.Instance.FadeImage));
    }
}
