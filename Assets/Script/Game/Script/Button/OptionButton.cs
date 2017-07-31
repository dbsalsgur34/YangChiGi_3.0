using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : GameButtonBase {

    private GameObject TempMenu;


    protected override void Start()
    {
        base.Start();
        TempMenu = GameObject.Find("TempMenu");
        TempMenu.SetActive(false);

        AddButtonClickEvent(OptionButtonEvent);
    }

    private void OptionButtonEvent()
    {
        if (GameTime.IsTimerStart())
        {
            if (TempMenu.activeSelf.Equals(false))
                TempMenu.SetActive(true);
            else
                TempMenu.SetActive(false);
        }
        AudioManager.Instance.PlayOneShotEffectClipByName("Button_InGame_Option");
    }
}
