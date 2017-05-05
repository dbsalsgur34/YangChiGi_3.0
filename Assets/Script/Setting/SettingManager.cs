using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ManagerBase {

    private Slider sound;
    private Text soundamount;
    private Button resetButton;
    private Button ShowDev;
    private GameObject Warning;
    private int quality;

    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {
        base.Start();
        sound = GameObject.Find("Slider").GetComponent<Slider>();
        soundamount = GameObject.Find("ShowSound").GetComponent<Text>();
        Warning = GameObject.Find("Warning");
        resetButton = GameObject.Find("Yes").GetComponent<Button>();
        this.Warning.SetActive(false);
        this.sound.value = PlayManage.Instance.Sound;

        resetButton.onClick.AddListener(DeleteAllData);
	}
	
	// Update is called once per frame
	public void Update () {
        SoundSetting();
	}

    private void SoundSetting()
    {
        PlayManage.Instance.Sound = this.sound.value;
        soundamount.text = sound.value.ToString("N0");
        
    }

    private void ActiveObject(GameObject target)
    {
        target.SetActive(true);
    }

    private void UnActiveObject(GameObject target)
    {
        target.SetActive(false);
    }

    private void DeleteAllData()
    {
        PlayManage.Instance.ResetData();
        this.sound.value = 50;
        this.Warning.SetActive(false);
    }
}
