using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public enum PlayerSearchState
{
    SHEEPSEARCH,
    ENEMYSEARCH,
    BACKTOHOME
}

public class PlayerControlThree : MonoBehaviour {

    private float sheepCount;
    public float SheepCount
    {
        get { return sheepCount; }
        set { if (value < 0) { sheepCount = 0; } else { sheepCount = value; } }
    }
    private float score;
    public float Score
    {
        get { return score; }
        set { if (value < 0) { score = 0; } else { score = value; } }
    }
    private float initialScore;
    //public float angle;
    
    public List<GameObject> SheepList;
    public GameObject HQ;
    public GameObject targetObject;
    public GameObject SheepArea;

    public class PlayerState
    {
        public bool InHQ { get; set; }
        public bool IsStop { get; set; }
        public bool IsBoost { get; set; }
        public bool IsFreeze { get; set; }
        public bool IsKnockBack { get; set; }

        public PlayerState()
        {
            InHQ = true;
            IsStop = false;
            IsBoost = false;
            IsFreeze = false;
            IsKnockBack = false;
        }
    }

    private PlayerState PS;

    public PlayerState GetPlayerState()
    {
        return this.PS;
    }

    private PlayerSearchState PSS;

    public PlayerSearchState GetPlayerSearchState()
    {
        return this.PSS;
    }

    private PlayerMovingInstance PMI;

    public PlayerMovingInstance GetPMI()
    {
        return this.PMI;
    }

    //플레이어의 이동과 관련된 class
    public class PlayerMovingInstance
    {
        public PlayerMovingInstance(Transform ParentOfPlayer, Transform curTarget, Transform prevTarget)
        {
            HorizontalInputValue = 0;
            VerticalInputValue = 0;
            knockBackPower = 10;
            curTransform = curTarget;
            prevTransform = prevTarget;
            playerParent = ParentOfPlayer;
            targetVector = Vector3.zero;
            initSpeed = 10;
            Speed = initSpeed;
            initTurnSpeed = 100;
            TurnSpeed = initTurnSpeed;
            minDistance = 5f;
        }

        public float HorizontalInputValue;
        public float VerticalInputValue;

        private float knockBackPower;           //10이 제일 적당
        public Transform curTransform;
        public Transform prevTransform;
        private Transform playerParent;
        public Vector3 targetVector;
        private bool IsKnockBack;
        private float time;
        public float Time
        {
            get{return time;}
            set { if (value < 0) { time = 0; } else { time = value; } }
        }
        private float initSpeed;                //10이 제일 적당
        private float speed;
        public float Speed
        {
            get { return speed; }
            set { if (value < 0) { speed = 0; } else { speed = value; } }
        }
        private float initTurnSpeed;            //100이 제일 적당.
        private float turnSpeed;
        public float TurnSpeed
        {
            get { return turnSpeed;}
            set { if (value < 0) { turnSpeed = 0; } else { turnSpeed = value; } }
        }
        private float minDistance;

        public Transform GetPlayerParent()
        {
            return playerParent;
        }

        public float GetMinDistance()
        {
            return minDistance;
        }

        public float GetKnockBackPower()
        {
            return knockBackPower;
        }
    }

    public void Start()
    {
        SheepArea = new GameObject("SheepArea");
        SheepArea.transform.position = this.transform.position;
        PlayerInstnaceInit();
    }

    public void SetPlayerState(PlayerSearchState newPSS)
    {
        this.PSS = newPSS;
    }
    
    private void PlayerInstnaceInit()
    {

        PMI = new PlayerMovingInstance(this.gameObject.transform.parent,SheepArea.transform,this.transform);
        PMI.GetPlayerParent().position = Vector3.zero;
        PMI.GetPlayerParent().rotation = HQ.transform.rotation;

        SheepCount = 0f;
        Score = 0f;
        initialScore = 0f;

        PSS = PlayerSearchState.BACKTOHOME;

        PS = new PlayerState();
     }

    /*private void PlayerInput()
    {
#if UNITY_EDITOR       //Unity Editor에서만!
        PMI.HorizontalInputValue = Input.GetAxisRaw(PMI.GetHorizontalControlName());
        PMI.VerticalInputValue = Input.GetAxisRaw(PMI.GetVerticalControlName());
#elif UNITY_ANDROID
        Vector3 tpos = Input.GetTouch(0).position;
        if (tpos.x < Screen.width / 2)
        {
            HorizontalInputValue = -1;
        }
        else if (tpos.x > Screen.width / 2)
        {
            HorizontalInputValue = 1;
        }
#endif
    }

    void KeyboardInput()
    {
        PlayerInput();
        if (HorizontalInputValue != 0)
        {
            Quaternion targetrotation = Quaternion.AngleAxis(angle * HorizontalInputValue, this.transform.up) * this.transform.rotation;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetrotation, turnspeed * Time.deltaTime);
        }
    }*/

    public void AddSheepList(GameObject Sheep)
    {
        SheepList.Add(Sheep);
    }

    public void ChangeMaster(GameObject Sheep, GameObject target)
    {
        int index = SheepList.IndexOf(Sheep);
        SheepList[index].GetComponent<SheepControlThree>().Master = target;
        target.GetComponent<PlayerControlThree>().AddSheepList(Sheep);
        SheepList.RemoveAt(index);
    }

    private void PlayerMove()
    {
        if (targetObject != null)
        {
            /*Vector3 targetvector = targetObject.transform.position - this.transform.position;
            Quaternion targetrotation = Quaternion.LookRotation(new Vector3(targetvector.x, targetvector.y, targetvector.z), this.transform.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetrotation, turnspeed * Time.deltaTime);
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);*/
            PMI.GetPlayerParent().transform.rotation = TurnToTarget();
            PMI.GetPlayerParent().transform.rotation *= GoStraight(PMI.Speed);
        }
    }

    Quaternion TurnToTarget()
    {
        float angle;
        Vector3 PO = this.gameObject.transform.position;
        Vector3 TO = targetObject.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(this.gameObject.transform.right, PTVector);
        Quaternion AA = Quaternion.AngleAxis(angle, PMI.GetPlayerParent().up) * PMI.GetPlayerParent().rotation;
        return Quaternion.RotateTowards(PMI.GetPlayerParent().rotation, AA, PMI.TurnSpeed * Time.deltaTime);
    }

    private Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(SP * Time.deltaTime, 0, 0));
    }

    private void LeaderSheep()
    {
        float dis = Vector3.Distance(PMI.prevTransform.position, PMI.curTransform.position);
        Vector3 newpos = PMI.prevTransform.position;

        float divisor = Mathf.Sqrt(PMI.GetMinDistance() * PMI.Speed);
        if (divisor == 0) { divisor = 1f; }
        float T = Time.fixedDeltaTime * dis / divisor;
        if (T > 0.5f)
            T = 0.5f;
        PMI.curTransform.position = Vector3.Slerp(PMI.curTransform.position, newpos, T);
        PMI.curTransform.rotation = Quaternion.Slerp(PMI.curTransform.rotation, PMI.prevTransform.rotation, T);
    }

    private float CalSheepScore()
    {
        float calscore = initialScore;

        foreach (GameObject i in SheepList)
        {
            calscore += i.GetComponent<SheepControlThree>().SheepScore;
        }

        return calscore;
    }

    private void CheckSheepType()
    {
        int bronze = 0;
        int sliver = 0;
        for (int i = 0; i < SheepList.Count; i++)
        {
            if (SheepList[i].tag == "BronzeSheep")
            {
                bronze++;
            }
            else if (SheepList[i].tag == "SliverSheep")
            {
                sliver++;
            }
        }

        /*if (bronze >= 5)
        {
            ChangeSheep("BronzeSheep", GM.silversheepprefab);
        }
        if (sliver >= 5)
        {
            ChangeSheep("SliverSheep", GM.goldensheepprefab);
        }*/
    }

    /*private void ChangeSheep(string targettag, GameObject targetSheep)
    {
        int ChangeCount = 5;
        for (int i = SheepList.Count - 1; ; i--)
        {
            if (ChangeCount == 5 && SheepList[i].tag == targettag)
            {
                GameObject newsheep = Instantiate(targetSheep, GM.Sheephorde.transform);
                SheepControlThree tempsheepcontrol = newsheep.GetComponent<SheepControlThree>();
                newsheep.transform.position = SheepList[i].transform.position;
                newsheep.transform.rotation = SheepList[i].transform.rotation;

                tempsheepcontrol.SetMaster(SheepList[i].GetComponent<SheepControlThree>().Master);
                tempsheepcontrol.CheckSheepState();
                GameObject tempsheep = SheepList[i];
                SheepList[i] = newsheep;
                tempsheep.SetActive(false);
                ChangeCount--;
            }

            else if (SheepList[i].tag == targettag && ChangeCount != 5)
            {
                GameObject followsheep;
                followsheep = SheepList[i + 1];
                if (i == 0)
                    followsheep.GetComponent<SheepControltwo>().leader = this.gameObject;
                else
                    followsheep.GetComponent<SheepControltwo>().leader = SheepList[i - 1];
                GameObject tempsheep = SheepList[i];
                SheepList.RemoveAt(i);
                tempsheep.SetActive(false);
                ChangeCount--;
            }
            if (ChangeCount == 0)
            {
                break;
            }
        }
    }*/

    private void SearchClosestSheep()
    {
        int Mincount = 0;
        float distance1;
        float distance2;
        if (PSS == PlayerSearchState.SHEEPSEARCH)
        {
            if (ManagerHandler.Instance.GameManager().GetSheepListCount() != 0)
            {
                for (int i = 1; i <= ManagerHandler.Instance.GameManager().GetSheepListCount() - 1; i++)
                {
                    distance1 = Vector3.Distance(this.transform.position, ManagerHandler.Instance.GameManager().GetSheepFromSheepList(Mincount).transform.position);
                    distance2 = Vector3.Distance(this.transform.position, ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).transform.position);
                    if (distance1 <= distance2)
                    {
                        continue;
                    }
                    else if (distance2 < distance1)
                    {
                        if (ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master != null && ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master.tag == "Dog" && ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master.GetComponent<Dog>().AreYouMyMaster(this.gameObject))
                            continue;
                        else
                            Mincount = i;
                    }
                }
                targetObject = ManagerHandler.Instance.GameManager().GetSheepFromSheepList(Mincount).gameObject;
            }
            else
            {
                targetObject = HQ;
            }
        }
        else if (PSS == PlayerSearchState.BACKTOHOME)
        {
            targetObject = HQ;
        }
        else if (PSS == PlayerSearchState.ENEMYSEARCH)
        {
            GameObject Target;
            if (ManagerHandler.Instance.GameManager().GetEnemy().gameObject.Equals(this.gameObject))
            {
                Target = ManagerHandler.Instance.GameManager().GetPlayer().gameObject;
            }
            else
            {
                Target = ManagerHandler.Instance.GameManager().GetEnemy().gameObject;
            }

            int tempcount = Target.GetComponent<PlayerControlThree>().SheepList.Count;
            if (tempcount == 0)
            {
                targetObject = Target;
            }
            else
            {
                targetObject = Target.GetComponent<PlayerControlThree>().SheepList[tempcount - 1];
            }
        }
    }

    private void SearchPhaseShift()
    {
        if (PSS == PlayerSearchState.SHEEPSEARCH)
        {
            PSS = PlayerSearchState.ENEMYSEARCH;
            targetObject = HQ;
        }
        else if (PSS == PlayerSearchState.BACKTOHOME)
        {
            PSS = PlayerSearchState.SHEEPSEARCH;
            SearchClosestSheep();
        }
        else if (PSS == PlayerSearchState.ENEMYSEARCH)
        {
            PSS = PlayerSearchState.BACKTOHOME;
            SearchClosestSheep();
        }
    }

    public IEnumerator EnterHQ()
    {
        if (SheepList.Count > 0)
        {
            for (int i = SheepList.Count - 1; i >= 0; i--)
            {
                SheepList[i].SetActive(false);
                Score++;
                yield return new WaitForSeconds(0.1f);
            }
            SheepList.RemoveRange(0,SheepList.Count);
        }
    }

    public IEnumerator PlayerFreeze(float freezeTime, float freezeSpeed)
    {
        this.PS.IsFreeze = true;
        float tempspeed = PMI.Speed;
        PMI.Speed /= freezeSpeed;
        yield return new WaitForSeconds(freezeTime);
        PMI.Speed *= freezeSpeed;
        this.PS.IsFreeze = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Head")
        {
            PMI.targetVector = (col.transform.position - this.gameObject.transform.position).normalized * PMI.GetKnockBackPower() ;
            StartCoroutine(SwitchKnockBack());
        }
    }

    private void KnockBack(Vector3 targetV)
    {
        this.gameObject.transform.localPosition = new Vector3(0, 26, 0);
        this.gameObject.transform.localRotation = Quaternion.identity;
        Vector3 knockBackVector = this.gameObject.transform.position - targetV;
        Quaternion Q = Quaternion.FromToRotation(this.gameObject.transform.position,knockBackVector) * PMI.GetPlayerParent().rotation;
        Quaternion QQ = Quaternion.Euler(Q.eulerAngles);
        float flowtime = Time.fixedTime - PMI.Time;
        PMI.GetPlayerParent().rotation = Quaternion.Slerp(PMI.GetPlayerParent().rotation, QQ, (1f-(Mathf.Sqrt(flowtime)))*Time.fixedDeltaTime);
    }

    private IEnumerator SwitchKnockBack()
    {
        PMI.Time = Time.fixedTime;
        this.PS.IsKnockBack = true;
        this.PS.IsStop = true;
        yield return new WaitForSeconds(1f);
        this.PS.IsKnockBack = false;
        this.PS.IsStop = false;
    }

    public void FixedUpdate()
    {
        if (!PS.IsStop && GameTime.IsTimerStart())
        {
            LeaderSheep();
            //KeyboardInput();
            SheepCount = CalSheepScore();
            //CheckSheepType();
            SearchClosestSheep();

            if ((PSS == PlayerSearchState.BACKTOHOME || ManagerHandler.Instance.GameManager().GetSheepListCount() == 0) && Vector3.Distance(this.gameObject.transform.position, this.HQ.transform.position) < 0.4)
            {
                return;
            }
            else
            {
                PlayerMove();
            }
        }

        if (PS.IsKnockBack)
        {
            KnockBack(PMI.targetVector);
        }
    }
}
