using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SheepState
{
    NOOWNER,
    HAVEOWNER
}

// 항상 일정 거리를 유지하도록!
public class SheepControl : MonoBehaviour
{
    // 공개 항목
    public GameObject leader;
    public GameObject Master;
    public PlayerControlThree player1;
    public PlayerControlThree player2;

    public int distance_permitted;
    public float speed;
    public float runspeed;
    public float limit_count;
    public SheepState SS;

    //비공개 항목
    public Quaternion lastLeaderQuaternion;
    public Vector3 lastLeaderPosition;
    public List<Quaternion> rotations;
    public List<Vector3> positions;
    // Use this for initialization

    private void Start()
    {
        if (this.leader != null)
        {
            AddLists();
        }
        player1 = GameObject.Find("PlayerOne").GetComponent<PlayerControlThree>();
        player2 = GameObject.Find("PlayerTwo").GetComponent<PlayerControlThree>();
    }

    void FollowLeader()
    {
        if (this.leader == null)
        {
            return;
        }

        if (SS == SheepState.HAVEOWNER)
        {
            if (lastLeaderQuaternion != rotations[rotations.Count - 1] && rotations.Count <= limit_count)
            {
                AddLists();
            }

            if (rotations.Count >= distance_permitted)
            {
                if (gameObject.transform.rotation != rotations[0])
                {
                    this.transform.position = Vector3.Slerp(transform.position, positions[0], Time.deltaTime * speed);
                    this.transform.rotation = Quaternion.Slerp(transform.rotation, rotations[0], Time.deltaTime*speed);
                }
                else
                {
                    rotations.Remove(rotations[0]);
                    positions.Remove(positions[0]);
                }
            }
            lastLeaderQuaternion = leader.transform.rotation;
            lastLeaderPosition = leader.transform.position;
        }
    }                      //리더를 따라가는 함수.

    void OnTriggerEnter(Collider col)       //부딪힌 오브젝트의 종류에 따른 반응 정리
    {
        if (col.gameObject.tag == "Head" && col.gameObject != this.Master)
        {
            CheckOwner(col.gameObject);
            ResetTarget();
        }
    }

    void CheckOwner(GameObject target)          //태그가 Head 인 오브젝트와 부딪혔을 시에 시행하는 함수
    {
        if (SS == SheepState.NOOWNER)
        {
            this.Master = target;
            ChangeLeader(target);
            SS = SheepState.HAVEOWNER;
            Master.GetComponent<PlayerControl>().AddSheepList(this.gameObject);
            AddLists();
        }
        else
        {
            ChangeLeader(target);
            Master.GetComponent<PlayerControl>().ChangeMaster(this.gameObject, target);
            rotations.Clear();
            positions.Clear();
            AddLists();
        }
    }

    void ChangeLeader(GameObject target)
    {
        if (target.GetComponent<PlayerControl>().SheepList.Count == 0)
        {
            this.leader = target;
        }
        else
        {
            this.leader = target.GetComponent<PlayerControl>().SheepList[target.GetComponent<PlayerControl>().SheepList.Count - 1];
        }
    }

    void AddLists()
    {
        if (positions.Count == 0 || (leader.transform.position - positions[positions.Count - 1]).magnitude > 0.1)
        {
            rotations.Add(leader.transform.rotation);
            positions.Add(leader.transform.position);
        }
    }

    void ResetTarget()
    {
        if (player1.TargetSheep == this.gameObject)
        {
            player1.TargetSheep = null;
        }
        if (player2.TargetSheep == this.gameObject)
        {
            player2.TargetSheep = null;
        }
    }

    void GoStraight()
    {
        this.transform.Translate(Vector3.forward * runspeed * Time.deltaTime);
    }

    // Update is called once per frame
    public void Update()
    {
        FollowLeader();
    }
}