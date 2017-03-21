using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ClientSide;

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
    public GameObject TempMenu;
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
            TempMenu = GameObject.Find("TempMenu");
            B.onClick.AddListener(OptionButtonEvent);
            TempMenu.SetActive(false);
        }
    }

    void SwitchPhase()
    {
        if (GM.IsGameStart())
        {
            Network_Client.Send("Shepherd/" + KingGodClient.Instance.Playernum +","+ (int)GM.Player.GetComponent<PlayerControlThree>().PS + "," + GM.ReturnTimePass());
            ChangeSearchButtonText();
        }
    }

    void ChangeSearchButtonText()
    {
        string searchtext = "Search";
        string backtohome = "Back";
        string enemytext = "Enemy";
        if (PCT.PS == PlayerState.BACKTOHOME)
        {
            ButtonText.text = searchtext;
        }
        else if (PCT.PS == PlayerState.SHEEPSEARCH)
        {
            ButtonText.text = enemytext;
        }
        else if (PCT.PS == PlayerState.ENEMYSEARCH)
        {
            ButtonText.text = backtohome;
        }
    }

    void SwitchCameraPhase()
    {
        if (GM.IsGameStart())
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
    }

    void OptionButtonEvent()
    {
        if (GM.IsGameStart())
        {
            TempMenu.SetActive(true);
        }
    }
}
