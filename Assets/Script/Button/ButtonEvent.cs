using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonEvent : MonoBehaviour {

    public enum ButtonEventType
    {
        LOBBYSCENEMOVE,
        LOBBYPLAY,
        MENUEXIT
    }

    public string targetScene;
    public LobbyManager LM;
    public bool IsSave;
    public ButtonEventType BET;
    public Button B;

    private void Start()
    {
        LM = GameObject.FindGameObjectWithTag("Manager").GetComponent<LobbyManager>();
        B = this.gameObject.GetComponent<Button>();
        if (BET == ButtonEventType.LOBBYSCENEMOVE)
        {
            B.onClick.AddListener(LobbyLoadScene);
            B.onClick.AddListener(SavePref);
        }
        else if (BET == ButtonEventType.LOBBYPLAY)
        {
            B.onClick.AddListener(Matching);
        }
        else if (BET == ButtonEventType.MENUEXIT)
        {
            B.onClick.AddListener(MenuLoadScene);
        }
    }
    void Matching()
    {
        LM.LoadingScene.SetActive(true);
        SendLMToCreateNetworkObject();
    }

    void LobbyLoadScene()
    {
        if (!LM.IsGameMatching)
        {
            StartCoroutine(PlayManage.Instance.LoadScene(targetScene));
        }
    }

    void MenuLoadScene()
    {
        StartCoroutine(PlayManage.Instance.LoadScene(targetScene));
    }

    void SavePref()
    {
        if (IsSave)
        {
            PlayManage.Instance.SaveData();
            PlayerPrefs.Save();
        }
    }

    void SendLMToCreateNetworkObject()
    {
        LM.CreateNetworkObject();
    }
}
