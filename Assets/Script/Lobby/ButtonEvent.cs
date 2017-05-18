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

    public string targetSceneName = null;
    public bool IsSave = false;
    public ButtonEventType BET;

    private Button B;
    private LobbyManager LM;
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
            B.onClick.AddListener(SavePref);
        }
    }
    private void Matching()
    {
        LM.SetLoadingScene(true);
        SendLMToCreateNetworkObject();
    }

    private void LobbyLoadScene()
    {
        if (!LM.RetrunIsGameMatching())
        {
            StartCoroutine(PlayManage.Instance.LoadScene(targetSceneName));
        }
    }

    private void MenuLoadScene()
    {
        StartCoroutine(PlayManage.Instance.LoadScene(targetSceneName));
    }

    private void SavePref()
    {
        if (IsSave)
        {
            PlayManage.Instance.SaveData();
            PlayerPrefs.Save();
        }
    }

    void SendLMToCreateNetworkObject()
    {
        LM.SendMessage("CreateNetworkObject");
    }
}
