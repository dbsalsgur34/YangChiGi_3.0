using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuButton : MonoBehaviour {

    public enum GameMenuButtonType
    {
        EXITMENU,
        SOUND
    }

    public GameMenuButtonType GBT;
    public GameObject TempMenu;

	// Use this for initialization
	void Start ()
    {
        Button B = this.gameObject.GetComponent<Button>();
        if (GBT == GameMenuButtonType.EXITMENU)
        {
            B.onClick.AddListener(ExitMenuEvent);
        }
        else if (GBT == GameMenuButtonType.SOUND)
        {
            B.onClick.AddListener(SoundEvent);
        }
	}

    void ExitMenuEvent()
    {
        TempMenu.SetActive(false);
    }

    void SoundEvent()
    {
        
    }

}
