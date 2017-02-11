using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : ManagerBase {

    public Text playerleveltext;
    public Text playerIDtext;
    public Text playerEXP;
    public GameObject clientObject;
    public GameObject LoadingScene;
    public Image targeticon;
    public Text MatchingMessage;
    public Button MatchingCancleButton;

    int level;
    float EXP;
    float maxEXP;
    float Iconrotation;

    private void Start()
    {
        LobbyInit();
        CalEXP();
    }

    void LobbyInit()
    {
        playerleveltext = GameObject.Find("PlayerLevel").GetComponent<Text>();
        playerIDtext = GameObject.Find("PlayerID").GetComponent<Text>();
        playerEXP = GameObject.Find("PlayerExp").GetComponent<Text>();
        level = PlayManage.Instance.playerlevel;
        EXP = PlayManage.Instance.EXP;
        if (level < 10)
        {
            playerleveltext.text = "Level : 0" + level.ToString();
        }
        else
        {
            playerleveltext.text = "Level : " + level.ToString();
        }
        playerIDtext.text = PlayManage.Instance.playerID;
        maxEXP = level * 1000;
        playerEXP.text = "EXP : " + PlayManage.Instance.EXP.ToString("N0") + " / " + maxEXP.ToString("N0");

        LoadingScene = GameObject.Find("Loading");
        targeticon = LoadingScene.GetComponentsInChildren<Image>()[1];
        MatchingMessage = LoadingScene.GetComponentsInChildren<Text>()[0];
        MatchingCancleButton = LoadingScene.GetComponentInChildren<Button>();
        MatchingCancleButton.onClick.AddListener(CancleMatching);
        LoadingScene.SetActive(false);
        StartCoroutine(TextBlink());
    }

    void TargetIconRotate()
    {
        Iconrotation += 75f * Time.deltaTime;
        targeticon.rectTransform.rotation = Quaternion.AngleAxis(Iconrotation, targeticon.rectTransform.forward);
    }

    IEnumerator TextBlink()
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


    void CancleMatching()
    {
        LoadingScene.SetActive(false);
    }

    void CalEXP()
    {
        if (EXP >= maxEXP)
        {
            EXP -= maxEXP;
            level += 1;
            PlayManage.Instance.playerlevel = this.level;
            PlayManage.Instance.EXP = this.EXP;
            LobbyInit();

        }
    }

    private void Update()
    {
        if (LoadingScene.activeSelf)
        {
            TargetIconRotate();
        }
    }
}
