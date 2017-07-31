using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ManagerBase {

    private Slider sound;
    private Text soundAmount;
    private Slider effectSound;
    private Text effectAmount;

    public Button resetButton;

    private int quality;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        sound = GameObject.Find("SoundSlider").GetComponent<Slider>();
        soundAmount = GameObject.Find("ShowBackGroundSound").GetComponent<Text>();
        effectSound = GameObject.Find("EffectSlider").GetComponent<Slider>();
        effectAmount = GameObject.Find("ShowEffectSound").GetComponent<Text>();

        this.sound.value = PlayManage.Instance.Sound;
        this.effectSound.value = PlayManage.Instance.EffectSound;
        resetButton.onClick.AddListener(DeleteAllData);
	}
	
	// Update is called once per frame
	public void Update () {
        SoundSetting();
        EffectSoundSetting();
	}

    private void SoundSetting()
    {
        PlayManage.Instance.Sound = this.sound.value;
        soundAmount.text = sound.value.ToString("N0");
    }

    private void EffectSoundSetting()
    {
        PlayManage.Instance.EffectSound = this.effectSound.value;

        effectAmount.text = effectSound.value.ToString("N0");
    }

    private void DeleteAllData()
    {
        PlayManage.Instance.SendMessage("ResetData");
        this.sound.value = 50;
        this.effectSound.value = 50;
    }
}
