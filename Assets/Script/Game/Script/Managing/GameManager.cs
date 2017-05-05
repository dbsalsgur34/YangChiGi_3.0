using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ClientSide;

public class GameManager : ManagerBase {

    public static GameManager GMInstance;

    //게임플레이에 관련된 변수.
    private GameObject Planet;
    private GameObject Enemy;
    private int PlayerNumber;
    private GameObject Player;
    public GameObject Sheephorde;
    GameObject bronzesheepprefab;
    //public GameObject silversheepprefab;
    //public GameObject goldensheepprefab;
    public GameObject BackGround;

    public CameraControl mainCamera;
    public SkillManager SM;
    public HQControl HQ;
    private RazorControl RC;
    private RazorControl CenterRC;
    public List<GameObject> SheepList;
    public List<GameObject> grassprefab;
    private float PlayerScore;
    private float EnemyScore;

    //Object 생성 관련된 변수들.
    public float PlanetScale;
    public int initialSheep;
    public float delayTime = 0.1f;

    bool IsSkillonthePlanet;
    int RazorPoint;

    public Vector3 hitVector
    {
        get;set;
    }

    private SpriteRenderer hitMarker;
    public GameObject hitMarkerParent;

    private GameUIManager GUIM;
    private float midTime;

    public PlayerControlThree GetPlayer()
    {
        return Player.GetComponent<PlayerControlThree>();
    }

    public PlayerControlThree GetEnemy()
    {
        return Enemy.GetComponent<PlayerControlThree>();
    }

    public override void Awake()
    {
        base.Awake();
        GMInstance = this;
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.Find("Sheephorde");
        BackGround = GameObject.Find("BackGround");
        bronzesheepprefab = Resources.Load<GameObject>("Prefab/Sheeps/BronzeSheep");
        RC = GameObject.FindGameObjectWithTag("Razor").GetComponent<RazorControl>();
        CenterRC = GameObject.FindGameObjectWithTag("CenterRazor").GetComponent<RazorControl>();
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

        GUIM = GameObject.FindGameObjectWithTag("UIManager").GetComponent<GameUIManager>();

        //오브젝트 생성.
        Random.InitState(KingGodClient.Instance.Seed);
        SheepSpawn(bronzesheepprefab, PlanetScale, initialSheep);
        GrassSpawn(grassprefab, 24.5f, 150);
        //스킬 이펙트 초기화
        hitMarkerInit();

    }

    public override void Start()
    {
        base.Start();
        HQ = Player.GetComponent<PlayerControlThree>().HQ.GetComponent<HQControl>();
        Network_Client.Send("Ready/" + KingGodClient.Instance.Playernum);
        midTime = 0;
    }

    private void SheepSpawn(GameObject sheepprefab,float scale, int number)   //양을 임의의 위치에 소환하는 메서드.
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

    private void GrassSpawn(List<GameObject> grasslist, float scale, int number)
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

    public IEnumerator GoToResultScene()
    {
        if (!GUIM.GetIsTimerStart())
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayManage.Instance.LoadScene("Result"));
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

        if (IsSkillCanUse())
        {
            RC.DrawCircle(this.Planet.transform.position, this.HQ.transform.position, hitVector,RazorPoint);
            CenterRC.DrawCenterCircle(this.Planet.transform.position, 25.5f);
            ShowhitMarker(hitVector);
        }

        if (GUIM.GetTimePass() - midTime > 5)
        {
            midTime = GUIM.GetTimePass();
            Network_Client.Send("Position/" + Player.transform.position +"," + Enemy.transform.position + "," + GUIM.GetTimePass());
            SheepSpawn(bronzesheepprefab, PlanetScale, 1);
        }
    }

    private IEnumerator SendMessageToSkillUse(int num, GameObject Player, GameObject Enemy, GameObject HQ, Vector3 HV,float useTime)
    {
        Vector3 targetVector = HQ.transform.position - HV;
        float angle = Mathf.Atan2(targetVector.x, targetVector.z) * Mathf.Rad2Deg;
        yield return new WaitUntil(() => GUIM.GetTimePass() >= (useTime + delayTime));
        SM.UsingSkill(num,Player,Enemy,this.Planet.transform, HQ.gameObject.transform,angle, HV);
    }

    public bool IsGameStart()
    {
        return (GUIM.GetIsTimerStart()) ? true : false;
    }

    public bool IsSkillCanUse()
    {
        return (IsGameStart() && IsSkillonthePlanet) ? true : false;
    }

    private void hitMarkerInit()
    {
        hitMarker = GameObject.FindGameObjectWithTag("HitMarker").GetComponent<SpriteRenderer>();
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        hitMarkerParent = new GameObject("hitMarkerParent");                //create a new gameObject
        hitMarker.sprite = Resources.Load<Sprite>("Image/Resource/UI/select");
        hitMarker.gameObject.transform.parent = hitMarkerParent.transform;                    //make this camera a child of the new gameObject
        hitMarkerParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
        hitMarkerParent.SetActive(false);
    }

    private void ShowhitMarker(Vector3 target)
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

    public void SetRazorActive(bool RCactive, bool CenterRCactive)
    {
        RC.gameObject.SetActive(RCactive);
        CenterRC.gameObject.SetActive(CenterRCactive);
    }

    public float GetPlayerScore()
    {
        return this.PlayerScore;
    }

    public float GetEnmyScore()
    {
        return this.EnemyScore;
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
                target.SendMessage("SearchPhaseShift");
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