using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobbyButton : ButtonBase {

    ManagerBase manager;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ManagerBase>();
        AddButtonClickEvent(RequestBackToLobby);
	}

    private void RequestBackToLobby()
    {
        if (manager.AllowBackToLobby())
        {
            StartCoroutine(PlayManage.Instance.LoadScene("Lobby"));
        }
    }
}
