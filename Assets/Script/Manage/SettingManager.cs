using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : ManagerBase {

    public Slider sound;
    public Text soundamount;
    public Button High;
    public Button Mid;
    public Button Low;
    public Button Reset;
    public Button ShowDev;
    public GameObject Warning;
    public int quality;

	// Use this for initialization
	void Start () {

        sound = GameObject.Find("Slider").GetComponent<Slider>();
        soundamount = GameObject.Find("ShowSound").GetComponent<Text>();
        Warning = GameObject.Find("Warning");

        this.Warning.SetActive(false);
        this.sound.value = PlayManage.Instance.sound;
        this.quality = PlayManage.Instance.Quality;

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

    public void QualitySetting(int q)
    {
        PlayManage.Instance.Quality = q;
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
