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

    private Button B;
    private GameManager GM;
    private PlayerControlThree PCT;
    public GameButtonType GBT;
    private GameObject TempMenu;
    private bool IsthisButtonActive;
    public bool IsSkillCanActive;

    private Text ButtonText;
    // Use this for initialization

    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        ButtonText = gameObject.GetComponentInChildren<Text>();
        GM = GameManager.GMInstance;
        PCT = GM.GetPlayer();

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

    private void SwitchPhase()
    {
        if (GM.IsGameStart())
        {
            KingGodClient.Instance.GetNetworkMessageSender().SendPlayerStateToServer(KingGodClient.Instance.Playernum, (int)GM.GetPlayer().GetPlayerSearchState(), GameUIManager.GUIMInstance.GetTimePass());
            ChangeSearchButtonText();
        }
    }

    private void ChangeSearchButtonText()
    {
        string searchtext = "Search";
        string backtohome = "Back";
        string enemytext = "Enemy";
        if (PCT.GetPlayerSearchState() == PlayerSearchState.BACKTOHOME)
        {
            ButtonText.text = searchtext;
        }
        else if (PCT.GetPlayerSearchState() == PlayerSearchState.SHEEPSEARCH)
        {
            ButtonText.text = enemytext;
        }
        else if (PCT.GetPlayerSearchState() == PlayerSearchState.ENEMYSEARCH)
        {
            ButtonText.text = backtohome;
        }
    }

    private void SwitchCameraPhase()
    {
        if (GM.IsGameStart())
        {
            string freetext = "Free";
            string HQtext = "HQ";
            string Playertext = "Player";

            if (GM.GetMainCamera().ReturnCameraState() == CameraState.FREE)
            {
                ButtonText.text = freetext;
            }
            else if (GM.GetMainCamera().ReturnCameraState() == CameraState.LOCKONHQ)
            {
                ButtonText.text = HQtext;
            }
            else if (GM.GetMainCamera().ReturnCameraState() == CameraState.LOCKONPLAYER)
            {
                ButtonText.text = Playertext;
            }
        }
    }

    private void OptionButtonEvent()
    {
        if (GM.IsGameStart())
        {
            TempMenu.SetActive(true);
        }
    }
}
