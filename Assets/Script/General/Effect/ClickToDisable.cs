using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDisable : MonoBehaviour {

    private void Update()
    {

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            DisableAction();
        }
    
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            DisableAction();
        }
#endif
    }

    void DisableAction()
    {
        this.gameObject.SetActive(false);
    }
}
