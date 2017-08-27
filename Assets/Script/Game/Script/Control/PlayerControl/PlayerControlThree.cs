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

    private enum PlayerSearchProcessState
    {
        Searching,
        Ready
    }

    private float initialScore;
    //public float angle;

    private PlayerHerdSheepControl playerHerdSheepControl;
    
    public HQControl HQ;
    public GameObject targetObject;
    private Color symbolColor;

    private PlayerSearchProcessState playerSearchProcessState;

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
        PS = GetComponent<PlayerState>();
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
        playerSearchProcessState = PlayerSearchProcessState.Ready;
        PS = new PlayerState();


        playerHerdSheepControl.InitHerdSheepBase(this,this.PMI.Speed, true);
    }

    public void SetSymbolColor(Color color)
    {
        this.symbolColor = color;
        this.HQ.SetHQMarkerColor(color);
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
            PMI.GetPlayerParent().transform.rotation *= GoStraight(PMI.Speed * PS.MultiplyValue);
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
        return Quaternion.RotateTowards(PMI.GetPlayerParent().rotation, AA, PMI.TurnSpeed * PS.MultiplyValue *Time.deltaTime);
    }

    private Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(SP * Time.deltaTime, 0, 0));
    }

    private void SearchTarget()
    {
        if (playerSearchProcessState.Equals(PlayerSearchProcessState.Searching))
        {
            return;
        }
        else
        {
            StartCoroutine(SearchTargetProcess());
        }
    }

    private IEnumerator SearchTargetProcess()
    {
        float Angle1;
        float Angle2;
        playerSearchProcessState = PlayerSearchProcessState.Searching;

        if (PSS == PlayerSearchState.SHEEPSEARCH)
        {
            int Mincount = 0;
            //양 추적 매커니즘. 주인 없는 양 중 가까운 양의 획득을 제일 우선시한다.
            if (ManagerHandler.Instance.GameManager().GetHordeSheepListCount() != 0)
            {
                for (int i = 1; i <= ManagerHandler.Instance.GameManager().GetHordeSheepListCount() - 1; i++)
                {
                    SheepControlThree checkingSheep = ManagerHandler.Instance.GameManager().GetSheepFromHordeSheepList(i);
                    Angle1 = Vector3.Angle(this.transform.position, ManagerHandler.Instance.GameManager().GetSheepFromHordeSheepList(Mincount).transform.position);
                    Angle2 = Vector3.Angle(this.transform.position,checkingSheep.transform.position);
                    if (Angle1 <= Angle2)
                    {
                        continue;
                    }
                    else if (Angle2 < Angle1)
                    {
                        Mincount = i;
                    }
                }
                targetObject = ManagerHandler.Instance.GameManager().GetSheepFromHordeSheepList(Mincount).gameObject;
            }
            else
            {
                targetObject = HQ.gameObject;
            }
        }
        else if (PSS == PlayerSearchState.BACKTOHOME)
        {
            //귀환 매커니즘. 쉽다.
            targetObject = HQ.gameObject;
        }
        else if (PSS == PlayerSearchState.ENEMYSEARCH)
        {
            // 적 추적시 매커니즘.
            GameObject Target;
            if (ManagerHandler.Instance.GameManager().GetEnemy().gameObject.Equals(this.gameObject))
            {
                Target = ManagerHandler.Instance.GameManager().GetPlayer().gameObject;
            }
            else
            {
                Target = ManagerHandler.Instance.GameManager().GetEnemy().gameObject;
            }
            targetObject = Target;
        }

        yield return null;
        playerSearchProcessState = PlayerSearchProcessState.Ready;
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
        //this.PS.IsFreeze = true;
        float tempspeed = PMI.Speed;
        PMI.Speed /= freezeSpeed;
        yield return new WaitForSeconds(freezeTime);
        PMI.Speed *= freezeSpeed;
        //this.PS.IsFreeze = false;
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
        //this.PS.IsKnockBack = true;

        yield return new WaitForSeconds(1f);
        //this.PS.IsKnockBack = false;

    }

    public void FixedUpdate()
    {
        if (GameTime.IsTimerStart())
        {
            SearchTarget();

            if ((PSS == PlayerSearchState.BACKTOHOME || ManagerHandler.Instance.GameManager().GetHordeSheepListCount() == 0) && Vector3.Distance(this.gameObject.transform.position, this.HQ.transform.position) < 0.4)
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
