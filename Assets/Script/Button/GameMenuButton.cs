using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuButton : MonoBehaviour {

    public enum GameMenuButtonType
    {
        EXITMENU,
        SOUND,
        SURRENDER,
        NOTSUR,
        YESSUR
    }

    public GameMenuButtonType GBT;
    public GameObject TempMenu;
    public GameObject CheckSurrender;
	// Use this for initialization
	void Start ()
    {
        Button B = this.gameObject.GetComponent<Button>();
        if (GBT == GameMenuButtonType.EXITMENU)
        {
            B.onClick.AddListener(ExitMenuEvent);
        }
        if (GBT == GameMenuButtonType.SURRENDER)
        {
            B.onClick.AddListener(SurrenderEvent);
        }
        if (GBT == GameMenuButtonType.NOTSUR)
        {
            B.onClick.AddListener(NotSurrenderEvent);
        }
        if (GBT == GameMenuButtonType.YESSUR)
        {
            B.onClick.AddListener(YesSurrenderEvent);
        }
	}

    void ExitMenuEvent()
    {
        TempMenu.SetActive(false);
    }

    void SurrenderEvent()
    {
        CheckSurrender.SetActive(true);
    }

    void NotSurrenderEvent()
    {
        TempMenu.SetActive(false);
        CheckSurrender.SetActive(false);
    }

    void YesSurrenderEvent()
    {
        return;
    }
    
}
