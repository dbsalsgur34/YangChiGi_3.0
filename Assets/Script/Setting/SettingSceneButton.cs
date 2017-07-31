using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSceneButton : ButtonBase {

    public GameObject targetWindow;

    protected override void Start()
    {
        base.Start();
        AddButtonClickEvent(OpenAndClose);
    }

    private void OpenAndClose()
    {
        if (targetWindow.activeSelf)
        {
            targetWindow.SetActive(false);
        }
        else
        {
            targetWindow.SetActive(true);
        }
        AudioManager.Instance.PlayOneShotEffectClipByName("Button_Lobby");
        Debug.Log(this.gameObject.name);
    }

    


}
