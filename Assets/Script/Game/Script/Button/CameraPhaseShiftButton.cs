using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPhaseShiftButton : GameButtonBase {

    private string freetext = "Free";
    private string HQtext = "HQ";
    private string Playertext = "Player";
    private CameraControl mainCamera;
    private Text ButtonText;

    protected override void Start()
    {
        base.Start();
        ButtonText = gameObject.GetComponentInChildren<Text>();
        AddButtonClickEvent(SwitchCameraPhase);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
    }

    private void SwitchCameraPhase()
    {
        if (GameTime.IsTimerStart())
        {
            mainCamera.SwitchCameraState();
            CameraState CS = mainCamera.ReturnCameraState();
            if (CS.Equals(CameraState.FREE))
            {
                ButtonText.text = freetext;
            }
            else if (CS.Equals(CameraState.LOCKONHQ))
            {
                ButtonText.text = HQtext;
            }
            else if (CS.Equals(CameraState.LOCKONPLAYER))
            {
                ButtonText.text = Playertext;
            }
        }

        AudioManager.Instance.PlayOneShotEffectClipByName("Button_InGame_Camera",0.75f);
    }
}
