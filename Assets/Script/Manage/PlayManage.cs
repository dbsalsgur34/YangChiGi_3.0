using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
public class PlayManage : ManagerBase {

    public static PlayManage Instance;
    Image FadeImage;
    SkillDataBase SDB;

    public string playerID;
    public int playerlevel;
    public float EXP;
    public float sound;

    public float PlayerScore;
    public float EnemyScore;
    public bool IsSoundOn;

    int[] SkillPreSet;

    FirebaseAuth auth;
    FirebaseDatabase DB;
    DatabaseReference userInfoReferrence;

    public override void Awake()                //싱글톤 오브젝트를 만들자!
    {
        PlayManageAwake();
        //auth = FirebaseAuth.DefaultInstance;
        //DB = FirebaseDatabase.DefaultInstance;
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

    void PlayManageAwake()
    {

        //Debug.Log(auth.CurrentUser.DisplayName);
        //userInfoReferrence = DB.RootReference.Child("UserInfo").Child(auth.CurrentUser.DisplayName).Reference; 
        //playerID = userInfoReferrence.Child("NickName").GetValueAsync().ToString();
        //playerlevel = Int32.Parse(userInfoReferrence.Child("Level").GetValueAsync().ToString());
        //EXP = Int32.Parse(userInfoReferrence.Child("Exp").GetValueAsync().ToString());
        //sound = Int32.Parse(userInfoReferrence.Child("Sound").GetValueAsync().ToString());
        //playerlevel = ;
        //EXP;
        //sound;
        SkillPreSet = new int[4];
        for (int i = 0; i < 4; i++)
        {
            SkillPreSet[i] = -1;
        }
        PlayerScore = 0;
        EnemyScore = 0;
        IsSoundOn = true;
        SDB = this.gameObject.GetComponent<SkillDataBase>();
    }

    public IEnumerator LoadScene(string name)
    {
        IEnumerator FO = FadeOut(FadeImage);
        StartCoroutine(FO);
        yield return new WaitUntil( () => FO.MoveNext() == false);
        FadeImage = null;
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
        PlayerPrefs.SetFloat("SOUND", sound);
        PlayerPrefs.SetFloat("EXP", EXP);
        string SkillPreSetString = SkillPreSet[0]+","+SkillPreSet[1]+","+SkillPreSet[2]+","+SkillPreSet[3];
        PlayerPrefs.SetString("SKILLPRESET",SkillPreSetString);
    }

    public void LoadData()
    {
        this.playerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.playerlevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.sound = PlayerPrefs.GetFloat("SOUND", 50);
        this.EXP = PlayerPrefs.GetFloat("EXP", 0);
        string[] SkillPreSetList = PlayerPrefs.GetString("SKILLPRESET", "1,2,3,4").Split(',');
        for (int i = 0; i < 4; i++)
        {
            this.SkillPreSet[i] = int.Parse(SkillPreSetList[i]);
        }
        SDB.SetRandomNumber(SkillPreSet);
    }

    public void ResetData()
    {
        this.playerID = "Beginner";
        this.playerlevel = 1;
        this.sound = 50;
        this.EXP = 0;
        this.SkillPreSet = new int[4] { 1,2,3,4 };
        SaveData();
    }

    public void SaveScore(float PS, float ES)
    {
        this.PlayerScore = PS;
        this.EnemyScore = ES;
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

    public Image ReturnFadeImage()
    {
        return FadeImage;
    }

    private void OnApplicationQuit()
    {
        //auth.SignOut();
        Debug.Log("Yes");
    }

    public SkillDataBase GetSkillDataBase()
    {
        return this.SDB;
    }
}
