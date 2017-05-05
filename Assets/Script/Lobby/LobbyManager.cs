using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientSide;

public class LobbyManager : ManagerBase {

    private Text playerleveltext;
    private Text playerIDtext;
    private Text playerEXP;
    private GameObject clientObject;
    private GameObject LoadingScene;
    private Image targeticon;
    private Text MatchingMessage;
    private Button MatchingCancleButton;

    int level;
    float EXP;
    float maxEXP;
    float Iconrotation;

    private GameObject NetworkObject;
    private bool IsGameMatching;

    public override void Start()
    {
        base.Start();
        LobbyInit();
        CalEXP();
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
        level = PlayManage.Instance.PlayerLevel;
        EXP = PlayManage.Instance.EXP;
        if (level < 10)
        {
            playerleveltext.text = "Level : 0" + level.ToString();
        }
        else
        {
            playerleveltext.text = "Level : " + level.ToString();
        }
        playerIDtext.text = PlayManage.Instance.PlayerID;
        maxEXP = level * 1000;
        playerEXP.text = "EXP : " + PlayManage.Instance.EXP.ToString("N0") + " / " + maxEXP.ToString("N0");
    }

    private void LobbyObjectInit()
    {
        LoadingScene = GameObject.Find("Loading");
        targeticon = LoadingScene.GetComponentsInChildren<Image>()[1];
        MatchingMessage = LoadingScene.GetComponentsInChildren<Text>()[0];
        MatchingCancleButton = LoadingScene.GetComponentInChildren<Button>();
        MatchingCancleButton.onClick.AddListener(CancleMatching);
        LoadingScene.SetActive(false);
        StartCoroutine(TextBlink());
        IsGameMatching = false;
    }

    private void TargetIconRotate()
    {
        Iconrotation += 75f * Time.deltaTime;
        targeticon.rectTransform.rotation = Quaternion.AngleAxis(Iconrotation, targeticon.rectTransform.forward);
    }

    private IEnumerator TextBlink()
    {
        float i;
        while (true)
        {
            for (i = 1f; i >= 0; i -= 0.01f)
            {
                Color color = new Vector4(255, 255, 255, i);
                MatchingMessage.color = color;
                yield return null;
            }

            for (i = 0f; i <= 1; i += 0.01f)
            {
                Color color = new Vector4(255, 255, 255, i);
                MatchingMessage.color = color;
                yield return null;
            }
        }
    }

    private void CancleMatching()
    {
        IsGameMatching = false;
        LoadingScene.SetActive(false);
        Network_Client.Send("Cancle");
        NetworkObject.SetActive(false);
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

    private void CreateNetworkObject()
    {
        IsGameMatching = true;
        if (KingGodClient.Instance == null)
        {
            NetworkObject = Instantiate(Resources.Load("Prefab/ETC/NetworkObject")) as GameObject;
        }
        else
        {
            NetworkObject.SetActive(true);
            Network_Client.Begin(KingGodClient.Instance.serverIP);
        }
    }

    public void SetLoadingScene(bool set)
    {
        LoadingScene.SetActive(set);
    }

    public bool RetrunIsGameMatching()
    {
        return this.IsGameMatching;
    }

    private void Update()
    {
        if (LoadingScene.activeSelf)
        {
            TargetIconRotate();
        }
    }
}
