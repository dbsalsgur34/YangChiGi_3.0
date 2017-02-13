using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameButtonType
{
    PHASESHIFTBUTTON,
    CAMERAPHASESHIFTBUTTON,
    OPTIONBUTTON
}

public class GameButtonEvent : MonoBehaviour {

    public Button B;
    public GameManager GM;
    public PlayerControlThree PCT;
    public GameButtonType GBT;

    public bool IsthisButtonActive;
    public bool IsSkillCanActive;

    public Text ButtonText;
    // Use this for initialization
    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        ButtonText = gameObject.GetComponentInChildren<Text>();
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        PCT = GM.Player.GetComponent<PlayerControlThree>();

        if (GBT == GameButtonType.PHASESHIFTBUTTON)
        {
            B.onClick.AddListener(SwitchPhase);
        }
        else if (GBT == GameButtonType.CAMERAPHASESHIFTBUTTON)
        {
            B.onClick.AddListener(SwitchCameraPhase);
        }
        else if (GBT == GameButtonType.OPTIONBUTTON)
        {

            B.onClick.AddListener(OptionButtonEvent);
        }
    }

    void SwitchPhase()
    {
        string searchtext = "Search";
        string backtohome = "Back";
        string enemytext = "Enemy";

        PCT.SearchPhaseShift();
        if (PCT.PS == PlayerState.BACKTOHOME)
        {
            ButtonText.text = backtohome;
        }
        else if (PCT.PS == PlayerState.SHEEPSEARCH)
        {
            ButtonText.text = searchtext;
        }
        else if (PCT.PS == PlayerState.ENEMYSEARCH)
        {
            ButtonText.text = enemytext;
        }
    }

    void SwitchCameraPhase()
    {
        string freetext = "Free";
        string HQtext = "HQ";
        string Playertext = "Player";

        if (GM.mainCamera.CS == CameraState.FREE)
        {
            ButtonText.text = HQtext;
            GM.mainCamera.CS = CameraState.LOCKONHQ;
        }
        else if (GM.mainCamera.CS == CameraState.LOCKONHQ)
        {
            ButtonText.text = Playertext;
            GM.mainCamera.CS = CameraState.LOCKONPLAYER;
        }
        else if (GM.mainCamera.CS == CameraState.LOCKONPLAYER)
        {
            GM.mainCamera.CS = CameraState.FREE;
            ButtonText.text = freetext;
        }
    }

    void OptionButtonEvent()
    {

    }
}
