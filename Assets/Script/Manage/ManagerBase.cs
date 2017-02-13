using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase : FadeInOut {

    public virtual void Awake()
    { }

    public virtual void Start()
    {
        PlayManage.Instance.SearchFadeImage();
        StartCoroutine(PlayManage.Instance.FadeIn(PlayManage.Instance.FadeImage));
    }
}
