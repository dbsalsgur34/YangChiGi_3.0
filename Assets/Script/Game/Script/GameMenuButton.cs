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
    private Button B;
    private bool isSoundMute;

	// Use this for initialization
	void Start ()
    {
        B = this.gameObject.GetComponent<Button>();
        if (GBT == GameMenuButtonType.EXITMENU)
        {
            B.onClick.AddListener(ExitMenuEvent);
        }
        else if (GBT == GameMenuButtonType.SOUND)
        {
            B.onClick.AddListener(SoundEvent);
            B.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Resource/Button/Color/speaker");
            isSoundMute = false;
        }
	}

    void ExitMenuEvent()
    {
        TempMenu.SetActive(false);
        AudioManager.Instance.PlayOneShotEffectClipByName("Button_InGame_Option");
    }

    void SoundEvent()
    {
        AudioManager.Instance.SwitchAudioMute();
        if (!isSoundMute)
        {
            isSoundMute = true;
            B.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Resource/Button/Color/speaker_off");
        }
        else
        {
            isSoundMute = false;
            B.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Resource/Button/Color/speaker");
        }
        AudioManager.Instance.PlayOneShotEffectClipByName("Button_InGame_Option");
    }

}
