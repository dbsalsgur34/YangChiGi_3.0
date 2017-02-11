using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManage : ManagerBase {

    public static PlayManage Instance;
    public Image FadeImage;

    public string playerID;
    public int playerlevel;
    public float speed;
    public float angle;
    public float score;
    public float distance;
    public float EXP;
    public float sound;
    public int Quality;

    public float PlayerScore;
    public float EnemyScore;

    public override void Awake()                //싱글톤 오브젝트를 만들자!
    {
        if (Instance == null)           //Static 변수를 지정하고 이것이 없을경우 - PlayManage 스크립트를 저장하고 이것이 전 범위적인 싱글톤 오브젝트가 된다.
        {
            DontDestroyOnLoad(this.gameObject);
            LoadData();
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);   //싱글톤 오브젝트가 있을경우 다른 오브젝트를 제거.
        }
        SearchFadeImage();
        StartCoroutine(FadeIn(FadeImage));
    }

    public IEnumerator LoadScene(string name)
    {
        IEnumerator FO = FadeOut(FadeImage);
        StartCoroutine(FO);
        yield return new WaitUntil( () => FO.MoveNext() == false);
        SceneManager.LoadScene(name);
    }

    public void SearchFadeImage()
    {
        FadeImage = GameObject.FindGameObjectWithTag("Fadescreen").GetComponent<Image>();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("PLAYERID", playerID);
        PlayerPrefs.SetInt("PLAYERLEVEL", playerlevel);
        PlayerPrefs.SetFloat("INITIALSPEED",speed);
        PlayerPrefs.SetFloat("INITIALANGLE", angle);
        PlayerPrefs.SetFloat("INITIALSCORE",score);
        PlayerPrefs.SetFloat("INITIALDISTANCE",distance);
        PlayerPrefs.SetFloat("SOUND", sound);
        PlayerPrefs.SetInt("QUALITY", Quality);
        PlayerPrefs.SetFloat("EXP", EXP);
    }

    public void LoadData()
    {
        this.playerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.playerlevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.speed = PlayerPrefs.GetFloat("INITIALSPEED",5);
        this.angle = PlayerPrefs.GetFloat("INITIALANGLE", 5);
        this.score = PlayerPrefs.GetFloat("INITIALSCORE", 0);
        this.distance = PlayerPrefs.GetFloat("INITIALDISTANCE", 15);
        this.sound = PlayerPrefs.GetFloat("SOUND", 50);
        this.Quality = PlayerPrefs.GetInt("QUALITY", 2);
        this.EXP = PlayerPrefs.GetFloat("EXP", 0);
    }

    public void ResetData()
    {
        this.playerID = "Beginner";
        this.playerlevel = 1;
        this.speed = 5;
        this.angle = 5;
        this.score = 0;
        this.distance = 15;
        this.sound = 50;
        this.Quality = 2;
        this.EXP = 0;
        SaveData();
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }
}
