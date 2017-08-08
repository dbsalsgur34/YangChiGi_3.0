using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class SheepControlThree : MonoBehaviour {

    public enum SheepState
    {
        NOOWNER,
        HAVEOWNER
    }

    private HerdSheepBase follower;
    private SpriteRenderer sheepSpriteRenderer;

    private SheepState SS;

    public float SmoothMove;
    public int SheepScore;


    private void Awake()
    {
        sheepSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.sheepSpriteRenderer;
    }

    private void OnTriggerEnter(Collider col)       //부딪힌 오브젝트에 HerdSheepBase가 존재하는지 확인.
    {
        HerdSheepBase target = col.GetComponent<HerdSheepBase>();
        if (target != null)
        {
            CheckSheepFollower(target);
        }
    }

    private void CheckSheepFollower(HerdSheepBase target)          //부딪힌 오브젝트에 HerdSheepBase가 존재할시 행동.
    {
        //양에 주인이 없을 경우.
        if (SS.Equals(SheepState.NOOWNER))
        {
            this.follower = target;
            SS = SheepState.HAVEOWNER;
            target.AddSheepList(this);
            ManagerHandler.Instance.GameManager().FromHordeSheepToOwneredSheep(this);
        }
        //양에게 주인이 있을경우 능동성 및 마스터 여부를 따져서 행동한다.
        else if(SS.Equals(SheepState.HAVEOWNER))
        {
            // Equals는 대상의 내용, ==는 대상의 주소를 비교한다.
            if (target.GetOwner() == this.follower.GetOwner())
            {
                //주인이 같은경우 행동하지 않는다.
                return;
            }
            else
            {
                if (!target.isTakeOverPermit())
                {
                    //주인이 다르나 탈취 권한이 없을경우 역시 행동하지 않는다.
                }
                else
                {
                    //주인도 다르고 탈취 권환도 있을경우 행동.
                    this.follower.ChangeMasterToTargetOwner(this, target);
                }
            }
        }
    }

    private void ResetTarget(GameObject col)
    {
        col.GetComponent<PlayerControlThree>().targetObject = null;
    }

    public HerdSheepBase Follower
    {
        get { return follower; }set { follower = value; }
    }

    public SheepState GetSheepState()
    {
        return this.SS;
    }

    public void CheckSheepState()
    {
        if (this.follower == null)
        {
            SS = SheepState.NOOWNER;
        }
        else {
            SS = SheepState.HAVEOWNER;
        }
    }


}
