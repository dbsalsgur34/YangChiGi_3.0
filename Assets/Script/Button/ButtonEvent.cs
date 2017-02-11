using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ClientSide;

public class ButtonEvent : MonoBehaviour {

    public string targetScene;
    public GameObject clientObject;
    public LobbyManager LM;
    public bool IsSave;
    public bool IsPlay;
    public Button B;

    private void Start()
    {
        LM = GameObject.FindGameObjectWithTag("Manager").GetComponent<LobbyManager>();
        B = this.gameObject.GetComponent<Button>();
        if (!IsPlay)
        {
            B.onClick.AddListener(LoadScene);
            B.onClick.AddListener(SavePref);
        }
        else
        {
            B.onClick.AddListener(Matching);
        }
    }
    void Matching()
    {
        clientObject.GetComponent<KingGodClient>().Matching();
        LM.LoadingScene.SetActive(true);
    }

    void LoadScene()
    {
        StartCoroutine(PlayManage.Instance.LoadScene(targetScene));
    }

    void SavePref()
    {
        if (IsSave == true)
        {
            PlayManage.Instance.SaveData();
            PlayerPrefs.Save();
        }
    }
}
