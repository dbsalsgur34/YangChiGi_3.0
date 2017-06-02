using System.Collections;
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

    public int[] skillPreSet;

    private FirebaseAuth auth;
    private FirebaseDatabase DB;
    private DatabaseReference userInfoReferrence;

    public override void Awake()                //싱글톤 오브젝트를 만들자!
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
        skillPreSet = new int[4];
        for (int i = 0; i < 4; i++)
        {
            skillPreSet[i] = 0;
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
        PlayerPrefs.SetInt("PLAYERLEVEL", this.playerLevel);
        PlayerPrefs.SetFloat("SOUND", this.Sound);
        PlayerPrefs.SetFloat("EXP", this.exp);
        string SkillPreSetString = skillPreSet[0]+","+skillPreSet[1]+","+skillPreSet[2]+","+skillPreSet[3];
        PlayerPrefs.SetString("SKILLPRESET",SkillPreSetString);
    }

    private void LoadData()
    {
        this.PlayerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.playerLevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.Sound = PlayerPrefs.GetFloat("SOUND", 50);
        this.exp = PlayerPrefs.GetFloat("EXP", 0);
        string[] SkillPreSetList = PlayerPrefs.GetString("SKILLPRESET", "1,2,3,4").Split(',');
        for (int i = 0; i < 4; i++)
        {
            this.skillPreSet[i] = int.Parse(SkillPreSetList[i]);
        }
        SDB.SetRandomNumber(skillPreSet);
        this.maxEXP = playerLevel * 1000;
    }

    private void ResetData()
    {
        this.PlayerID = "Beginner";
        this.playerLevel = 1;
        this.PlayerScore = 0;
        this.EnemyScore = 0;
        this.Sound = 50;
        this.exp = 0;
        this.skillPreSet = new int[4] { 1,2,3,4 };
        SaveData();
    }

    public IEnumerator SetSkillPreSet(int index, int skillNum)
    {
        this.skillPreSet[index] = skillNum;
        yield return null;
    }

    public int GetSkillPreSet(int index)
    {
        return skillPreSet[index];
    }

    public SkillDataBase GetSkillDataBase()
    {
        return this.SDB;
    }
}
