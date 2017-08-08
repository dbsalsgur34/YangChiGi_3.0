using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : ButtonBase {

    public enum ButtonEventType
    {
        TRAININGSCENEMOVE,
        SETTINGSCENEMOVE,
        PLAY,
        CANCLE
    }

    public ButtonEventType BET;

    private LobbyManager LM;

    protected override void Start()
    {
        base.Start();
        LM = GameObject.FindGameObjectWithTag("Manager").GetComponent<LobbyManager>();
        switch (BET)
        {
            case ButtonEventType.TRAININGSCENEMOVE:
                AddButtonClickEvent(() => LobbyLoadScene("Training"));
                AddButtonClickEvent(SavePref);
                break;
            case ButtonEventType.SETTINGSCENEMOVE:
                AddButtonClickEvent(() => LobbyLoadScene("Setting"));
                AddButtonClickEvent(SavePref);
                break;
            case ButtonEventType.PLAY:
                AddButtonClickEvent(() => LM.StartMatching());
                AddButtonClickEvent(SavePref);
                break;
             case ButtonEventType.CANCLE:
                AddButtonClickEvent(() => LM.CancleMatching());
                AddButtonClickEvent(SavePref);
                break;
        }
    }

    private void LobbyLoadScene(string targetScene)
    {
        if (!LM.GetIsGameMatching())
        {
            StartCoroutine(PlayManage.Instance.LoadScene(targetScene));
        }
    }

    private void SavePref()
    {
        PlayManage.Instance.SaveData();
        PlayerPrefs.Save();
    }
}
