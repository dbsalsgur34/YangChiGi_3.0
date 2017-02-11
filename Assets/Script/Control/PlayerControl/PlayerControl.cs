using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    //공개 항목
    public int PlayerNumber;
    public float Score;
    public float InitialScore;
    public float speed;
    public float angle;
    public float turnspeed;
    public float mindistance;
    public bool IsBoost;
    public GameManager GM;
    public List<GameObject> SheepList;

    //비공개 항목
    public bool IsgameOver;
    string HorizontalControlName;
    string VerticalControlName;
    float HorizontalInputValue;
    float VerticalInputValue;
    Transform curtransform;
    Transform prevtransform;

    public void Start()
    {
        HorizontalControlName = "Horizontal" + PlayerNumber;
        VerticalControlName = "Vertical" + PlayerNumber;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.speed = PlayManage.Instance.speed;
        this.angle = PlayManage.Instance.angle;
        this.InitialScore = PlayManage.Instance.score;
        this.mindistance = PlayManage.Instance.distance;

        IsgameOver = false;
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

    void KeyboardInput()
    {
        PlayerInput();
        if (HorizontalInputValue != 0)
        {
            Quaternion targetrotation = Quaternion.AngleAxis(angle * HorizontalInputValue, this.transform.up) * this.transform.rotation;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetrotation, turnspeed * Time.deltaTime);
        }
    }

    public void AddSheepList(GameObject Sheep)
    {
        SheepList.Add(Sheep);
    }

    public void ChangeMaster(GameObject Sheep, GameObject target)
    {
        int index = SheepList.IndexOf(Sheep);

        for (int temp = index; temp <= SheepList.Count - 1; temp++)
        {
            SheepList[temp].GetComponent<SheepControlThree>().Master = target;
            target.GetComponent<PlayerControlThree>().SheepList.Add(this.SheepList[temp]);
        }
        SheepList.RemoveRange(index, SheepList.Count - index);
    }

    void GoStraight()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void LeaderSheep()
    {
        for (int i = 0; i < SheepList.Count; i++)
        {
            if (i == 0)
            {
                curtransform = SheepList[i].transform;
                prevtransform = this.transform;
            }
            else
            {
                curtransform = SheepList[i].transform;
                prevtransform = SheepList[i - 1].transform;
            }

            float dis = Vector3.Distance(prevtransform.position, curtransform.position);
            Vector3 newpos = prevtransform.position;

            float T = Time.deltaTime * dis / mindistance * speed;
            if (T > 0.5f)
                T = 0.5f;
            curtransform.position = Vector3.Slerp(curtransform.position, newpos, T);
            curtransform.rotation = Quaternion.Slerp(curtransform.rotation, prevtransform.rotation, T);
        }
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

        if (bronze >= 5)
        {
            ChangeSheep("BronzeSheep", GM.silversheepprefab);
        }
        if (sliver >= 5)
        {
            ChangeSheep("SliverSheep", GM.goldensheepprefab);
        }

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
                //tempsheepcontrol.leader = SheepList[i].GetComponent<SheepControltwo>().leader;
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
                    //followsheep.GetComponent<SheepControltwo>().leader = this.gameObject;
                else
                    //followsheep.GetComponent<SheepControltwo>().leader = SheepList[i - 1];
                GameObject tempsheep = SheepList[i];
                SheepList.RemoveAt(i);
                tempsheep.SetActive(false);
                ChangeCount--;
                */
            }
            if (ChangeCount == 0)
            {
                break;
            }

        }
    }

    void AfterBoost(float targetTime, float DurationTime)
    {
        if (IsBoost == true && targetTime >= DurationTime)
        {
            this.speed = 10;
            this.angle = 6.7f;
            this.IsBoost = false;
        }
    }

    public void FixedUpdate()
    {
        if (IsgameOver == false)
        {
            LeaderSheep();
            GoStraight();
            KeyboardInput();
            Score = CalSheepScore();
            CheckSheepType();
            AfterBoost(Time.fixedTime, 60f);
        }
    }
}
