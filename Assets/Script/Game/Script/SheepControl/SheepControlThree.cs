using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class SheepControlThree : MonoBehaviour {

    // 공개 항목
    //public GameObject leader;
    private GameObject master;

    private GameObject player1;
    private GameObject player2;

    private SheepState SS;

    public float SmoothMove;
    public int SheepScore;

    // Use this for initialization

    private void Start()
    {
        player1 = GameObject.Find("PlayerOne");
        player2 = GameObject.Find("PlayerTwo");

    }

    private void OnTriggerEnter(Collider col)       //부딪힌 오브젝트의 종류에 따른 반응 정리
    {
        if (col.gameObject.tag == "Head" && col.gameObject != this.Master)
        {
            CheckOwner(col.gameObject);
            ResetTarget(col.gameObject);
        }
    }

    private void CheckOwner(GameObject target)          //태그가 Head 인 오브젝트와 부딪혔을 시에 시행하는 함수
    {
        if (SS == SheepState.NOOWNER)
        {
            this.Master = target;
            //ChangeLeader(target);
            SS = SheepState.HAVEOWNER;
            Master.GetComponent<PlayerControlThree>().AddSheepList(this.gameObject);
            this.transform.parent = Master.GetComponent<PlayerControlThree>().SheepArea.transform;
            SetthisLocalPosition();
            GameManager.GMInstance.FindAndRemoveAtSheepList(this.gameObject);
        }
        else
        {
            //ChangeLeader(target);
            if (Master.gameObject.tag == "Head")
            {
                Master.GetComponent<PlayerControlThree>().ChangeMaster(this.gameObject, target);
                this.transform.parent = target.GetComponent<PlayerControlThree>().SheepArea.transform;
                SetthisLocalPosition();
            }
            else if (Master.gameObject.tag == "Dog" && !Master.GetComponent<Dog>().AreYouMyMaster(target))
            {
                Master.GetComponent<Dog>().ChangeMaster(this, target.GetComponent<PlayerControlThree>());
                ResetTarget(target.gameObject);
            }
        }
    }

    public void SetthisLocalPosition()
    {
        //Random.InitState(KingGodClient.Instance.Seed);
        Vector2 Circleposition = Random.insideUnitCircle * 3;
        this.transform.localPosition = new Vector3(Circleposition.x, 0, Circleposition.y);
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void ResetTarget(GameObject col)
    {
        col.GetComponent<PlayerControlThree>().targetObject = null;
    }

    public bool AreYouMyMaster(GameObject target)
    {
        return (target.Equals(this.Master)) ? true : false;
    }

    public GameObject Master
    {
        get { return master; }set { master = value; }
    }

    public SheepState GetSheepState()
    {
        return this.SS;
    }

    public void CheckSheepState()
    {
        if (this.Master == null)
        {
            SS = SheepState.NOOWNER;
        }
        else {
            SS = SheepState.HAVEOWNER;
        }
    }


}
