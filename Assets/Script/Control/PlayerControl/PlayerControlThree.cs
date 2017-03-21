using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public enum PlayerState
{
    SHEEPSEARCH,
    ENEMYSEARCH,
    BACKTOHOME
}

public class PlayerControlThree : MonoBehaviour {

    //공개 항목
    public int PlayerNumber;
    public float SheepCount;
    public float Score;
    public float InitialScore;
    public float speed;
    //public float angle;
    public float turnspeed;
    public float mindistance;
    public GameManager GM;
    public List<GameObject> SheepList;
    public GameObject HQ;
    public GameObject targetObject;
    public GameObject SheepArea;
    public bool InHQ;
    public bool IsgameOver;
    public bool IsBoost;
    public bool IsFreeze;
    public PlayerState PS;
    public float knockBackPower = 10;
    //비공개 항목
    string HorizontalControlName;
    string VerticalControlName;
    float HorizontalInputValue;
    float VerticalInputValue;
    Transform curtransform;
    Transform prevtransform;
    Transform playerParent;
    Vector3 targetVector;
    bool IsKnockBack;
    float time;

    public void Start()
    {

        PlayerInstnaceInit();
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        SheepArea = new GameObject("SheepArea");
        SheepArea.transform.position = this.transform.position;

        playerParent = this.transform.parent;
        playerParent.transform.position = Vector3.zero;
        playerParent.transform.rotation = HQ.transform.rotation;
    }

    float Initspeed = 10f;
    float Initturnspeed = 100f;

    void PlayerInstnaceInit()
    {
        HorizontalControlName = "Horizontal" + PlayerNumber;
        VerticalControlName = "Vertical" + PlayerNumber;
        
        speed = Initspeed;
        turnspeed = Initturnspeed;
        mindistance = 15f;
        SheepCount = 0f;
        Score = 0f;
        InitialScore = 0f;
        IsgameOver = false;
        PS = PlayerState.BACKTOHOME;
        IsBoost = false;
        IsKnockBack = false;
     }

    public void PlayerInput()
    {
#if UNITY_EDITOR       //Unity Editor에서만!
        HorizontalInputValue = Input.GetAxisRaw(HorizontalControlName);
        VerticalInputValue = Input.GetAxisRaw(VerticalControlName);
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

    /*void KeyboardInput()
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

    void PlayerMove()
    {
        if (targetObject != null)
        {
            /*Vector3 targetvector = targetObject.transform.position - this.transform.position;
            Quaternion targetrotation = Quaternion.LookRotation(new Vector3(targetvector.x, targetvector.y, targetvector.z), this.transform.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetrotation, turnspeed * Time.deltaTime);
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);*/
            this.playerParent.transform.rotation = TurnToTarget();
            this.playerParent.transform.rotation *= GoStraight(speed);
        }
    }

    Quaternion TurnToTarget()
    {
        float angle;
        Vector3 PO = this.gameObject.transform.position;
        Vector3 TO = targetObject.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(this.gameObject.transform.right, PTVector);
        Quaternion AA = Quaternion.AngleAxis(angle, playerParent.transform.up) * this.playerParent.transform.rotation;
        return Quaternion.RotateTowards(this.playerParent.transform.rotation, AA, turnspeed * Time.deltaTime);
    }

    Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(SP * Time.deltaTime, 0, 0));
    }

    void LeaderSheep()
    {
        curtransform = SheepArea.transform;
        prevtransform = this.transform;

        float dis = Vector3.Distance(prevtransform.position, curtransform.position);
        Vector3 newpos = prevtransform.position;

        float T = Time.deltaTime * dis / mindistance * speed;
        if (T > 0.5f)
            T = 0.5f;
        curtransform.position = Vector3.Slerp(curtransform.position, newpos, T);
        curtransform.rotation = Quaternion.Slerp(curtransform.rotation, prevtransform.rotation, T);
    }

    float CalSheepScore()
    {
        float calscore = InitialScore;

        foreach (GameObject i in SheepList)
        {
            calscore += i.GetComponent<SheepControlThree>().SheepScore;
        }

        return calscore;
    }

    void CheckSheepType()
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

    void ChangeSheep(string targettag, GameObject targetSheep)
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

                tempsheepcontrol.Master = SheepList[i].GetComponent<SheepControlThree>().Master;
                tempsheepcontrol.SS = SheepState.HAVEOWNER;
                GameObject tempsheep = SheepList[i];
                SheepList[i] = newsheep;
                tempsheep.SetActive(false);
                ChangeCount--;
            }

            else if (SheepList[i].tag == targettag && ChangeCount != 5)
            {
                GameObject followsheep;
                followsheep = SheepList[i + 1];
                /*if (i == 0)
                    followsheep.GetComponent<SheepControltwo>().leader = this.gameObject;
                else
                    followsheep.GetComponent<SheepControltwo>().leader = SheepList[i - 1];*/
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
    }

    void SearchClosestSheep()
    {
        int Mincount = 0;
        float distance1;
        float distance2;
        if (PS == PlayerState.SHEEPSEARCH)
        {
            if (GM.SheepList.Count != 0)
            {
                for (int i = 1; i <= GM.SheepList.Count - 1; i++)
                {
                    distance1 = Vector3.Distance(this.transform.position, GM.SheepList[Mincount].transform.position);
                    distance2 = Vector3.Distance(this.transform.position, GM.SheepList[i].transform.position);
                    if (distance1 <= distance2)
                    {
                        continue;
                    }
                    else if (distance2 < distance1)
                    {
                        if (GM.SheepList[i].GetComponent<SheepControlThree>().Master != null && GM.SheepList[i].GetComponent<SheepControlThree>().Master.tag == "Dog" && GM.SheepList[i].GetComponent<SheepControlThree>().Master.GetComponent<Dog>().Owner == this.gameObject)
                            continue;
                        else
                            Mincount = i;
                    }
                }
                targetObject = GM.SheepList[Mincount];
            }
            else
            {
                targetObject = HQ;
            }
        }
        else if (PS == PlayerState.BACKTOHOME)
        {
            targetObject = HQ;
        }
        else if (PS == PlayerState.ENEMYSEARCH)
        {
            GameObject Target;
            if (GM.Enemy == this.gameObject)
            {
                Target = GM.Player;
            }
            else
            {
                Target = GM.Enemy;
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

    public void SearchPhaseShift()
    {
        if (PS == PlayerState.SHEEPSEARCH)
        {
            PS = PlayerState.ENEMYSEARCH;
            targetObject = HQ;
        }
        else if (PS == PlayerState.BACKTOHOME)
        {
            PS = PlayerState.SHEEPSEARCH;
            SearchClosestSheep();
        }
        else if (PS == PlayerState.ENEMYSEARCH)
        {
            PS = PlayerState.BACKTOHOME;
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
        IsFreeze = true;
        float tempspeed = this.speed;
        this.speed /= freezeSpeed;
        yield return new WaitForSeconds(freezeTime);
        this.speed *= freezeSpeed;
        IsFreeze = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Head")
        {
            targetVector = (col.transform.position - this.gameObject.transform.position).normalized * knockBackPower;
            StartCoroutine(SwitchKnockBack());
        }
    }

    void KnockBack(Vector3 targetV)
    {
        this.gameObject.transform.localPosition = new Vector3(0, 26, 0);
        this.gameObject.transform.localRotation = Quaternion.identity;
        Vector3 knockBackVector = this.gameObject.transform.position - targetV;
        Quaternion Q = Quaternion.FromToRotation(this.gameObject.transform.position,knockBackVector) * this.playerParent.rotation;
        Quaternion QQ = Quaternion.Euler(Q.eulerAngles);
        float flowtime = Time.fixedTime - time;
        this.playerParent.rotation = Quaternion.Slerp(this.playerParent.rotation, QQ, (1f-(Mathf.Sqrt(flowtime)))*Time.fixedDeltaTime);
    }

    IEnumerator SwitchKnockBack()
    {
        time = Time.fixedTime;
        IsKnockBack = true;
        IsgameOver = true;
        yield return new WaitForSeconds(1f);
        IsKnockBack = false;
        IsgameOver = false;
    }

    public void FixedUpdate()
    {
        if (IsgameOver == false && GM.IsGameStart())
        {
            LeaderSheep();
            //KeyboardInput();
            SheepCount = CalSheepScore();
            //CheckSheepType();
            SearchClosestSheep();

            if ((PS == PlayerState.BACKTOHOME || GM.SheepList.Count == 0) && Vector3.Distance(this.gameObject.transform.position, this.HQ.transform.position) < 0.4)
            {
                return;
            }
            else
            {
                PlayerMove();
            }
        }

        if (IsKnockBack)
        {
            KnockBack(targetVector);
        }
    }
}
