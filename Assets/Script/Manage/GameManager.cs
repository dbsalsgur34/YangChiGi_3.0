using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ClientSide;

public class GameManager : ManagerBase {

    GameObject Planet;
    Text UItext;
    Text UIscore;
    Text UIcurrentSheep;
    Text UIEnemyScore;
    GameObject EndScreen;
    Text EndText;

    public GameObject Enemy;
    float SheepCount;
    float PlayerScore;
    float EnemyScore;
    int PlayerNumber;

    public GameObject Player;
    public GameObject Sheephorde;
    public GameObject bronzesheepprefab;
    //public GameObject silversheepprefab;
    //public GameObject goldensheepprefab;
    public GameObject BackGround;

    public CameraControl mainCamera;
    public SkillManager SM;
    public HQControl HQ;
    public RazorControl RC;
    public RazorControl CenterRC;
    public List<GameObject> SheepList;

    public List<GameObject> grassprefab;

    public float PlanetScale;
    public float initialtime;
    public int initialSheep;
    public float delayTime = 0.1f;
    float Timer;
    bool TimerStart;
    bool IsSkillonthePlanet;
    int RazorPoint;

    public Vector3 hitVector;
    public SpriteRenderer hitMarker;
    public GameObject hitMarkerParent;

    public static GameManager GMInstance;

    int startTime = 0;
    float midTime = 0;

    public override void Awake()
    {
        base.Awake();
        GMInstance = this;
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.Find("Sheephorde");
        BackGround = GameObject.Find("BackGround");
        SM = GameObject.Find("SkillManager").GetComponent<SkillManager>();
        this.PlayerNumber = KingGodClient.Instance.Playernum;
        if (PlayerNumber == 1)
        {
            Player = GameObject.Find("PlayerOne");
            Enemy = GameObject.Find("PlayerTwo");
            RazorPoint = 1;
        }
        else if (PlayerNumber == 2)
        {
            Player = GameObject.Find("PlayerTwo");
            Enemy = GameObject.Find("PlayerOne");
            RazorPoint = -1;
        }

        //UI관련 초기화.
        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIscore = GameObject.Find("ScoreText").GetComponent<Text>();
        UIcurrentSheep = GameObject.Find("CurrentSheepText").GetComponent<Text>();
        UIEnemyScore = GameObject.Find("EnemyScoreText").GetComponent<Text>();

        EndScreen = GameObject.Find("EndScreen");
        EndScreen.SetActive(true);
        EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.gameObject.SetActive(false);

        SheepCount = 0;
        Timer = 0;
        TimerStart = false;

        //오브젝트 생성.
        Random.InitState(KingGodClient.Instance.Seed);
        SheepSpawn(bronzesheepprefab, PlanetScale, initialSheep);
        GrassSpawn(grassprefab, 24.5f, 150);
        //스킬 이펙트 관련 초기화
        hitMarkerInit();
    }

    public override void Start()
    {
        base.Start();
        HQ = Player.GetComponent<PlayerControlThree>().HQ.GetComponent<HQControl>();
        Network_Client.Send("Ready/" + KingGodClient.Instance.Playernum);
    }

    public bool ReturnTimerStart()
    {
        return TimerStart;
    }   //bool TimerStart 리턴.

    public float ReturnTimePass()
    {
        return (Timer - startTime);
    }

    void Showremainingtime()
    {
        string timetext;
        if (initialtime - Timer >= 0)
        {
            timetext = "Left Time : " + (initialtime - Timer).ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            finishgame();
        }

        UItext.text = timetext;
    }

    void ShowScore()
    {
        string scoretext;
        PlayerScore = Player.GetComponent<PlayerControlThree>().Score;
        if (PlayerScore >= 10)
        {
            scoretext = "My Score : " + PlayerScore;
        }
        else
        {
            scoretext = "My Score : 0" + PlayerScore;
        }
        UIscore.text = scoretext;
    }

    void ShowMySheep()
    {
        string scoretext;
        SheepCount = Player.GetComponent<PlayerControlThree>().SheepCount;
        if (SheepCount >= 10)
        {
            scoretext = "Current Sheep : " + SheepCount;
        }
        else
        {
            scoretext = "Current Sheep : 0" + SheepCount;
        }
        UIcurrentSheep.text = scoretext;
    }

    void ShowEnemyScore()
    {
        string scoretext;
        EnemyScore = Enemy.GetComponent<PlayerControlThree>().Score;
        if (EnemyScore >= 10)
        {
            scoretext = "Enemy Score : " + EnemyScore;
        }
        else
        {
            scoretext = "Enemy Score : 0" + EnemyScore;
        }
        UIEnemyScore.text = scoretext;
    }

    void ShowUIText()
    {
        Showremainingtime();
        ShowScore();
        ShowEnemyScore();
        ShowMySheep();
    }

    void finishgame()
    {
        if (this.TimerStart)
        {
            StartCoroutine("FinishRoutine");
        }
    }

    void SheepSpawn(GameObject sheepprefab,float scale, int number)   //양을 임의의 위치에 소환하는 메서드.
    {
        
        for (int i = 0; i < number; i++)
        {
            Vector3 newposition = Random.onUnitSphere * scale;
            if (Vector3.Distance(newposition, Player.transform.position) > 2 && Vector3.Distance(newposition, Enemy.transform.position) > 2)
            {
                GameObject tempSheep = Instantiate(sheepprefab, newposition, Quaternion.Euler(0, 0, 0), Sheephorde.transform);
                tempSheep.transform.rotation = Quaternion.FromToRotation(tempSheep.transform.up, newposition) * tempSheep.transform.rotation;
                SheepList.Add(tempSheep);
            }
            else
            {
                i--;
            }
        }
    }

    void GrassSpawn(List<GameObject> grasslist, float scale, int number)
    {
        int listcount = grasslist.Count-1;
        for (int i = 0; i < number; i++)
        {
            int index = Random.Range(0, listcount);
            Vector3 newposition = Random.onUnitSphere * scale;
            GameObject tempgrass = Instantiate(grasslist[index], newposition, Quaternion.Euler(0, 0, 0), BackGround.transform);
            tempgrass.transform.rotation = Quaternion.FromToRotation(tempgrass.transform.up, newposition) * tempgrass.transform.rotation;
        }    
    }

    public IEnumerator ReadyScreen()
    {
        Player.GetComponent<PlayerControlThree>().IsgameOver = true;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = true;
        EndText.gameObject.SetActive(true);
        EndText.text = "Ready...";
        yield return new WaitForSeconds(3f);
        Network_Client.Send("started");
        startTime = 0;
        EndScreen.SetActive(false);
        Player.GetComponent<PlayerControlThree>().IsgameOver = false;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = false;
        this.TimerStart = true;
        yield return 0;
    }

    IEnumerator FinishRoutine()
    {
        EndScreen.SetActive(true);
        Text EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.text = "Time Over!";
        Player.GetComponent<PlayerControlThree>().IsgameOver = true;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = true;
        PlayManage.Instance.PlayerScore = this.PlayerScore;
        PlayManage.Instance.EnemyScore = this.EnemyScore;
        Network_Client.Send("GameOver/" + this.PlayerNumber + "," + ReturnTimePass());
        this.TimerStart = false;
        yield return null;
    }

    public IEnumerator GoToResultScene()
    {
        if (!TimerStart)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayManage.Instance.LoadScene("Result"));
        }
    }

    void TimerSet()
    {
        if (TimerStart)
        {
            Timer += Time.deltaTime;
        }
    }

    public void FindAndRemoveAtSheepList(GameObject target)
    {
        int index;
        index = SheepList.FindIndex(x => x.gameObject == target);
        SheepList.RemoveAt(index);
    }

    /*public void SkillButtonAction(Button targetbutton)
    {
        if (SelectedButton == null)
        {
            SelectedButton = targetbutton;
            SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
        }
        else
        {
            if (SelectedButton == targetbutton)
            {
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
                SelectedButton = null;
            }
            else
            {
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
                SelectedButton = targetbutton;
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
            }
        }
    }

    void CheckCameraFix()
    {
        if (SelectedButton != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillIndex != 0 && SkillDB.SkillPrefab[SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillIndex].GetComponentInChildren<SkillBase>().FindSkillNeedCameraFix())
        {
            mainCamera.IsSkillCutScene = true;
            HQ.Arrow.gameObject.SetActive(true);
        }
        else
        {
            mainCamera.IsSkillCutScene = false;
            HQ.Arrow.gameObject.SetActive(false);
        }
    }

    void SkillCooltime()
    {
        if (TimerStart && skillcooltimer >= 0)
        {
            skillcooltimer -= Time.deltaTime;
        }
        if (skillcooltimer < 0)
        {
            if (!SkillButtonList[0].IsSkillCanActive || !SkillButtonList[1].IsSkillCanActive)
            {
                if (!SkillButtonList[0].IsSkillCanActive)
                {
                    SkillButtonList[0].IsSkillCanActive = true;
                    SkillDB.ButtonIconInRandomList(SkillButtonList[0].gameObject.GetComponent<Button>(), Skillindexcount);
                    CalSkillIndexCount(SkillButtonList[0]);
                    skillcooltimer = 10f;

                }
                else if (!SkillButtonList[1].IsSkillCanActive)
                {
                    SkillButtonList[1].IsSkillCanActive = true;
                    SkillDB.ButtonIconInRandomList(SkillButtonList[1].gameObject.GetComponent<Button>(), Skillindexcount);
                    CalSkillIndexCount(SkillButtonList[1]);
                    skillcooltimer = 10f;
                }
            }
        }
    }

    void CalSkillIndexCount(GameButtonEvent B)
    {
        B.SkillIndex = SkillDB.SkillIndexList[Skillindexcount];
        calSIC();
    }

    void calSIC()
    {
        if (Skillindexcount < SkillDB.SkillIndexList.Count - 1)
        {
            Skillindexcount++;
        }
        else
            Skillindexcount = 0;

        if (SkillDB.SkillIndexList[Skillindexcount] != SkillButtonList[0].SkillIndex && SkillDB.SkillIndexList[Skillindexcount] != SkillButtonList[1].SkillIndex)
        {
            return;
        }
        else { calSIC(); }
    }

    public void SkillFireAction()
    {
        if (SelectedButton.gameObject.GetComponent<GameButtonEvent>() == null || !SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
            return;
        else if (SelectedButton.gameObject.GetComponent<GameButtonEvent>() != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
        {
            GameButtonEvent GBEtemp = SelectedButton.gameObject.GetComponent<GameButtonEvent>();
            check = false;
            SelectedButton.image.sprite = SkillDB.SkillIcon[0];
            GBEtemp.IsSkillCanActive = false;
            ActivatedSkillPrefab = Instantiate(SkillDB.SkillPrefab[GBEtemp.SkillIndex]);
            GBEtemp.SkillIndex = 0;

            check = ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SetInstance(this.Player, this.Enemy);

            if (HQ.Arrow.gameObject.activeSelf)
            {
                ActivatedSkillPrefab.transform.rotation = this.HQ.ArrowPivot.rotation;
                HQ.Arrow.gameObject.SetActive(false);
            }
            else
                ActivatedSkillPrefab.transform.rotation = this.HQ.gameObject.transform.rotation;

            if (check)
                ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SS = SkillState.LAUNCHED;
            else
            {
                while (check)
                {
                    check = ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SetInstance(this.Player, this.Enemy);
                }
                ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SS = SkillState.LAUNCHED;
            }
        }
    }
    */

    // Update is called once per frame
    void FixedUpdate()
    {
        Application.runInBackground = true;
        ShowUIText();
        TimerSet();
        if (IsSkillCanUse())
        {
            RC.DrawCircle(this.Planet.transform.position, this.HQ.transform.position, hitVector,RazorPoint);
            CenterRC.DrawCenterCircle(this.Planet.transform.position, 25.5f);
            ShowhitMarker(hitVector);
        }

        if (Timer - midTime > 5)
        {
            midTime = Timer;
            Network_Client.Send("Position/" + Player.transform.position +"," + Enemy.transform.position + "," + ReturnTimePass());
            SheepSpawn(bronzesheepprefab, PlanetScale, 1);
        }
    }

    IEnumerator SendMessageToSkillUse(int num, GameObject Player, GameObject Enemy, GameObject HQ, Vector3 HV,float useTime)
    {
        Vector3 targetVector = HQ.transform.position - HV;
        float angle = Mathf.Atan2(targetVector.x, targetVector.z) * Mathf.Rad2Deg;
        yield return new WaitUntil(() => this.ReturnTimePass() >= (useTime + delayTime));
        SM.UsingSkill(num,Player,Enemy,this.Planet.transform, HQ.gameObject.transform,angle, HV);
    }

    public bool IsGameStart()
    {
        return (TimerStart) ? true : false;
    }

    public bool IsSkillCanUse()
    {
        return (IsGameStart() && IsSkillonthePlanet) ? true : false;
    }

    void hitMarkerInit()
    {
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        hitMarkerParent = new GameObject("hitMarkerParent");                //create a new gameObject
        hitMarker.gameObject.transform.parent = hitMarkerParent.transform;                    //make this camera a child of the new gameObject
        hitMarkerParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
        hitMarkerParent.SetActive(false);
    }

    void ShowhitMarker(Vector3 target)
    {
        if (IsSkillCanUse())
        {
            hitMarkerParent.SetActive(true);
            Quaternion targetRotation = Quaternion.FromToRotation(hitMarkerParent.transform.up, target) * hitMarkerParent.transform.rotation;
            hitMarkerParent.transform.rotation = targetRotation;
        }
        else
        {
            hitMarkerParent.SetActive(false);
        }
    }

    public void SetIsSkillOnthePlanaet(bool TF)
    {
        IsSkillonthePlanet = TF;
    }

    public void GetMessage(string MessageType , string Message)
    {
        string[] MessageArray = Message.Split(',');
        PlayerControlThree target;
        PlayerControlThree Opposite;
        if (int.Parse(MessageArray[0]) == this.PlayerNumber)
        {
            target = Player.GetComponent<PlayerControlThree>();
            Opposite = Enemy.GetComponent < PlayerControlThree>();
        }
        else
        {
            target = Enemy.GetComponent<PlayerControlThree>();
            Opposite = Player.GetComponent<PlayerControlThree>();
        }
        Debug.Log(MessageArray[0]);
        switch (MessageType)
        {
            case "Shepherd" :
                target.GetComponent<PlayerControlThree>().SearchPhaseShift();
                break;
            case "Skill":
                Vector3 skillVector = new Vector3(float.Parse(MessageArray[2]), float.Parse(MessageArray[3]), float.Parse(MessageArray[4]));
                StartCoroutine(SendMessageToSkillUse(int.Parse(MessageArray[1]),target.gameObject,Opposite.gameObject,target.HQ, skillVector, float.Parse(MessageArray[5])));
                break;
            case "Out":
                target.PS = PlayerState.BACKTOHOME;
                break;
        }
    }
}