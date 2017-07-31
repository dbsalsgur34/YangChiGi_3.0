using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class PlayManage : MonoBehaviour {

    public static PlayManage Instance;
    private int playerLevel = 1;
    private float playerScore;
    private float enemyScore;
    private float exp;
    private float sound = 50;
    private float effectSound = 50;
    private UIBaseManage UIB;
    private string skillPreSetList;
    private float maxEXP;

    public string PlayerID
    {
        get;set;
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public float GetEXP()
    {
        return exp;
    }

    public float GetMaxEXP()
    {
        return maxEXP;
    }

    public void CalLevel_EXP(float getEXP)
    {
        exp += getEXP;
        if (exp >= maxEXP)
        {
            this.playerLevel += 1;
            exp -= maxEXP;
        }
        SaveData();
    }

    public float Sound
    {
        get { return sound; }
        set { if (value < 0) { sound = 0; } else { sound = value; } }
    }

    public float EffectSound
    {
        get { return effectSound; }
        set { if (value < 0) { effectSound = 0; } else { effectSound = value; } }
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

    public string SkillPreSet
    {
        get { return skillPreSetList; }
        set { skillPreSetList = value; }
    }

    private bool isSoundOn;

    private FirebaseAuth auth;
    private FirebaseDatabase DB;
    private DatabaseReference userInfoReferrence;

    protected void Awake()                //싱글톤 오브젝트를 만들자!
    {
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
        PlayerScore = 0;
        EnemyScore = 0;
        isSoundOn = true;
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
        PlayerPrefs.SetInt("PLAYERLEVEL", this.playerLevel);
        PlayerPrefs.SetFloat("SOUND", this.sound);
        PlayerPrefs.SetFloat("EFFECT", this.effectSound);
        PlayerPrefs.SetFloat("EXP", this.exp);
        PlayerPrefs.SetString("SKILLPRESET",skillPreSetList);
    }

    private void LoadData()
    {
        this.PlayerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.playerLevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.sound = PlayerPrefs.GetFloat("SOUND", 50);
        this.effectSound = PlayerPrefs.GetFloat("EFFECT", 50);
        this.exp = PlayerPrefs.GetFloat("EXP", 0);
        this.skillPreSetList = PlayerPrefs.GetString("SKILLPRESET", "1,2,3,4");
        this.maxEXP = playerLevel * 1000;
    }

    private void ResetData()
    {
        this.PlayerID = "Beginner";
        this.playerLevel = 1;
        this.PlayerScore = 0;
        this.EnemyScore = 0;
        this.Sound = 50;
        this.effectSound = 50;
        this.exp = 0;
        this.skillPreSetList = "1,2,3,4";
        SaveData();
    }
}

