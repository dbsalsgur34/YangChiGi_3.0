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

[RequireComponent(typeof(PlayerHerdSheepControl))]

public class PlayerControlThree : MonoBehaviour {

    private float initialScore;
    //public float angle;

    private PlayerHerdSheepControl playerHerdSheepControl;
    
    public HQControl HQ;
    public GameObject targetObject;
    private Color symbolColor;

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
        public PlayerMovingInstance(Transform ParentOfPlayer)
        {
            HorizontalInputValue = 0;
            VerticalInputValue = 0;
            knockBackPower = 10;
            playerParent = ParentOfPlayer;
            targetVector = Vector3.zero;
            initSpeed = 10;
            Speed = initSpeed;
            initTurnSpeed = 100;
            TurnSpeed = initTurnSpeed;
        }

        public float HorizontalInputValue;
        public float VerticalInputValue;

        private float knockBackPower;           //10이 제일 적당
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

        public Transform GetPlayerParent()
        {
            return playerParent;
        }

        public float GetKnockBackPower()
        {
            return knockBackPower;
        }
    }

    private void Awake()
    {
        playerHerdSheepControl = GetComponent<PlayerHerdSheepControl>();
    }

    private void Start()
    {
        PlayerInstnaceInit();
    }

    public HerdSheepBase GetHerdSheepControl()
    {
        return this.playerHerdSheepControl;
    }

    public void SetPlayerState(PlayerSearchState newPSS)
    {
        this.PSS = newPSS;
    }
    
    //PlayerControl에 필요한 요소 초기화.
    private void PlayerInstnaceInit()
    {
        PMI = new PlayerMovingInstance(this.gameObject.transform.parent);
        PMI.GetPlayerParent().position = Vector3.zero;
        PMI.GetPlayerParent().rotation = HQ.transform.rotation;
        HQ.SetOwner(this);

        PSS = PlayerSearchState.BACKTOHOME;

        PS = new PlayerState();


        playerHerdSheepControl.InitHerdSheepBase(this,this.PMI.Speed, true);
    }

    public void SetSymbolColor(Color color)
    {
        this.symbolColor = color;
    }

    public Color GetSymbolColor()
    {
        return this.symbolColor;
    }

    private void PlayerMove()
    {
        if (targetObject != null)
        {
            PMI.GetPlayerParent().transform.rotation = TurnToTarget();
            PMI.GetPlayerParent().transform.rotation *= GoStraight(PMI.Speed);
        }
    }

    private Quaternion TurnToTarget()
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

    private void SearchTarget()
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
                        /*if (ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master != null && ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master.tag == "Dog" && ManagerHandler.Instance.GameManager().GetSheepFromSheepList(i).Master.GetComponent<Dog>().AreYouMyMaster(this))
                            continue;
                        else
                            Mincount = i;*/
                    }
                }
                targetObject = ManagerHandler.Instance.GameManager().GetSheepFromSheepList(Mincount).gameObject;
            }
            else
            {
                targetObject = HQ.gameObject;
            }
        }
        else if (PSS == PlayerSearchState.BACKTOHOME)
        {
            targetObject = HQ.gameObject;
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

            //int tempcount = Target.GetComponent<PlayerControlThree>().SheepList.Count;
            int tempcount = 0;  //일단 임시로
            if (tempcount == 0)
            {
                targetObject = Target;
            }
            else
            {
                //targetObject = Target.GetComponent<PlayerControlThree>().GetPlayerSheepList()[tempcount - 1].gameObject;
            }
        }
    }

    private void SearchPhaseShift()
    {
        if (PSS == PlayerSearchState.SHEEPSEARCH)
        {
            PSS = PlayerSearchState.ENEMYSEARCH;
            targetObject = HQ.gameObject;
        }
        else if (PSS == PlayerSearchState.BACKTOHOME)
        {
            PSS = PlayerSearchState.SHEEPSEARCH;
            SearchTarget();
        }
        else if (PSS == PlayerSearchState.ENEMYSEARCH)
        {
            PSS = PlayerSearchState.BACKTOHOME;
            SearchTarget();
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
            SearchTarget();

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
