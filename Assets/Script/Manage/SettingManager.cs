﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ManagerBase {

    public Slider sound;
    public Text soundamount;
    public Button Reset;
    public Button ShowDev;
    public GameObject Warning;
    public int quality;

	// Use this for initialization
	public override void Start () {
        base.Start();
        sound = GameObject.Find("Slider").GetComponent<Slider>();
        soundamount = GameObject.Find("ShowSound").GetComponent<Text>();
        Warning = GameObject.Find("Warning");

        this.Warning.SetActive(false);
        this.sound.value = PlayManage.Instance.sound;

        Reset.onClick.AddListener(DeleteAllData);
	}
	
	// Update is called once per frame
	void Update () {
        SoundSetting();
	}

    void SoundSetting()
    {
        soundamount.text = sound.value.ToString("N0");
        PlayManage.Instance.sound = this.sound.value;
    }

    public void ActiveObject(GameObject target)
    {
        target.SetActive(true);
    }

    public void UnActiveObject(GameObject target)
    {
        target.SetActive(false);
    }

    public void DeleteAllData()
    {
        PlayManage.Instance.ResetData();
        this.sound.value = 50;
        this.Warning.SetActive(false);
    }
}
