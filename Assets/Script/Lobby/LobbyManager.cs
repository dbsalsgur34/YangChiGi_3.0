using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientSide;

public class LobbyManager : ManagerBase {

    private enum LobbyState
    {
        IDLE,
        WAITFORPLAYING
    }

    private Text playerleveltext;
    private Text playerIDtext;
    private Text playerEXP;
    //private GameObject clientObject;
    private GameObject LoadingScene;
    private Button MatchingCancleButton;
    private LobbyState lobbyState;

    private int level;
    private float EXP;
    private float maxEXP;


    protected new void Awake()
    {
        return;
    }

    protected override void Start()
    {
        base.Start();
        LobbyInit();
        CalEXP();
        AudioManager.Instance.PlayBackGroundClipByName("Lobby",14.5f);

    }

    private void LobbyInit()
    {
        LobbyUIInit();
        LobbyObjectInit();        
    }

    private void LobbyUIInit()
    {
        playerleveltext = GameObject.Find("PlayerLevel").GetComponent<Text>();
        playerIDtext = GameObject.Find("PlayerID").GetComponent<Text>();
        playerEXP = GameObject.Find("PlayerExp").GetComponent<Text>();
        level = PlayManage.Instance.GetPlayerLevel();
        EXP = PlayManage.Instance.GetEXP();
        if (level < 10)
        {
            playerleveltext.text = "Level : 0" + level.ToString();
        }
        else
        {
            playerleveltext.text = "Level : " + level.ToString();
        }
        playerIDtext.text = PlayManage.Instance.PlayerID;
        maxEXP = PlayManage.Instance.GetMaxEXP();
        playerEXP.text = "EXP : " + EXP.ToString("N0") + " / " + maxEXP.ToString("N0");
        this.lobbyState = LobbyState.IDLE;
    }

    private void LobbyObjectInit()
    {
        LoadingScene = GameObject.Find("Loading");

        MatchingCancleButton = LoadingScene.GetComponentInChildren<Button>();
        MatchingCancleButton.onClick.AddListener(CancleMatching);
        LoadingScene.SetActive(false);
    }


    

    public void StartMatching()
    {
        if (this.lobbyState.Equals(LobbyState.IDLE))
        {
            this.lobbyState = LobbyState.WAITFORPLAYING;
            LoadingScene.SetActive(true);
            AudioManager.Instance.PlayEffectClipByName("Wait", 2f);
            AudioManager.Instance.PlayOneShotEffectClipByName("Button_Lobby");
            AudioManager.Instance.BackGroundClipAttenuation();
            NetworkObjectAction();
        }
    }

    public void CancleMatching()
    {
        if (this.lobbyState.Equals(LobbyState.WAITFORPLAYING))
        {
            this.lobbyState = LobbyState.IDLE;
            LoadingScene.SetActive(false);
            AudioManager.Instance.InitEffectAudio();
            AudioManager.Instance.BackGroundClipRestore();
            AudioManager.Instance.PlayOneShotEffectClipByName("Button_Lobby");
            Network_Client.StopThread();
        }
    }

    private void CalEXP()
    {
        if (EXP >= maxEXP)
        {
            EXP -= maxEXP;
            level += 1;
            PlayManage.Instance.SaveData();
            LobbyInit();
        }
    }

    private void NetworkObjectAction()
    {
        if (KingGodClient.Instance == null)
        {
            Instantiate(Resources.Load("Prefab/ETC/NetworkObject"));
        }
        else
        {
            Network_Client.Begin(KingGodClient.Instance.serverIP);
        }
    }

    public void SetLoadingScene(bool set)
    {
        LoadingScene.SetActive(set);
    }

    public bool GetIsGameMatching()
    {
        return (LoadingScene.activeSelf.Equals(true)) ? true : false;
    }
}
