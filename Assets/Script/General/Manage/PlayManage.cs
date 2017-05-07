﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class PlayManage : ManagerBase {

    public static PlayManage Instance;
    private int playerLevel = 1;
    private float playerScore;
    private float enemyScore;
    private float exp;
    private float sound = 50;
    private SkillDataBase SDB;
    private UIBaseManage UIB;


    public string PlayerID
    {
        get;set;
    }

    public int PlayerLevel
    {
        get { return playerLevel; }
        set { if (value < 1) { playerLevel = 1; } else { playerLevel = value; }  }
    }

    public float EXP
    {
        get { return exp; }
        set { if (value < 0) { exp = 0; } else { exp = value; } }
    }

    public float Sound
    {
        get { return sound; }
        set { if (value < 0) { sound = 0; } else { sound = value; } }
    }

    public float PlayerScore
    {
        get { return playerScore; }
        set { if (value < 0) { playerScore = 0; } else { playerScore = value; } }
    }

    public float EnemyScore
    {
        get { return enemyScore; }
        set { if (value < 0) { enemyScore = 0; } else { enemyScore = value; } }
    }

    private bool isSoundOn;

    public int[] SkillPreSet;

    private FirebaseAuth auth;
    private FirebaseDatabase DB;
    private DatabaseReference userInfoReferrence;

    public override void Awake()                //싱글톤 오브젝트를 만들자!
    {
        //auth = FirebaseAuth.DefaultInstance;
        //DB = FirebaseDatabase.DefaultInstance;
        PlayManageAwake();

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
        UIB = GameObject.FindGameObjectWithTag("UIBase").GetComponent<UIBaseManage>();
    }

    private void PlayManageAwake()
    {
        FireBaseInit();
        SkillPreSet = new int[4];
        for (int i = 0; i < 4; i++)
        {
            SkillPreSet[i] = -1;
        }
        PlayerScore = 0;
        EnemyScore = 0;
        isSoundOn = true;
        SDB = this.gameObject.GetComponent<SkillDataBase>();
    }

    private void FireBaseInit()
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
    }

    public IEnumerator LoadScene(string name)
    {
        StartCoroutine(UIB.LoadSceneAndFadeInOut(name));
        yield return null;
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("PLAYERID", this.PlayerID);
        PlayerPrefs.SetInt("PLAYERLEVEL", this.PlayerLevel);
        PlayerPrefs.SetFloat("SOUND", this.Sound);
        PlayerPrefs.SetFloat("EXP", this.EXP);
        string SkillPreSetString = SkillPreSet[0]+","+SkillPreSet[1]+","+SkillPreSet[2]+","+SkillPreSet[3];
        PlayerPrefs.SetString("SKILLPRESET",SkillPreSetString);
    }

    private void LoadData()
    {
        this.PlayerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.PlayerLevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.Sound = PlayerPrefs.GetFloat("SOUND", 50);
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
        this.PlayerID = "Beginner";
        this.PlayerLevel = 1;
        this.PlayerScore = 0;
        this.EnemyScore = 0;
        this.Sound = 50;
        this.EXP = 0;
        this.SkillPreSet = new int[4] { 1,2,3,4 };
        SaveData();
    }

    public SkillDataBase GetSkillDataBase()
    {
        return this.SDB;
    }
}
