using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApllicationManage : MonoBehaviour {

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    private void OnApplicationQuit()
    {
        //auth.SignOut();
        Debug.Log("Yes");
    }
}
