using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class NetworkManager : GameManagerBase
{
    //Network에서 받은 메세지를 저장하는 Queue.
    private Queue<string> messageQueue;
    private int playerNumber;
    public float delayTime = 0.1f;

    private NetworkMessageReceiver NMR;
    private NetworkMessageSender NMS;

    protected override void Start()
    {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
        messageQueue = new Queue<string>();
    }

    protected override void InitManager()
    {
        base.InitManager();
        playerNumber = KingGodClient.Instance.playerNum;
        delayTime = 0.1f;
        NMR = new NetworkMessageReceiver();
        NMS = new NetworkMessageSender();
        NMR.SetLogReference(KingGodClient.Instance.Seed);
        NMS.SetMatchReference(KingGodClient.Instance.Seed);
        NMS.SendReady(playerNumber);
    }

    public NetworkMessageReceiver GetNetworkMessageReceiver()
    {
        return this.NMR;
    }

    public NetworkMessageSender GetNetworkMessageSender()
    {
        return this.NMS;
    }

    public void SetMessageQueue(string Message)
    {
        if (GameTime.IsTimerStart())
            messageQueue.Enqueue(Message);
    }

    private IEnumerator MessageActor(string Message)
    {
        string[] messageSplit = Message.Split('/');
        string messageType = messageSplit[0];
        string[] MessageArray = messageSplit[1].Split(',');
        float targetTime = float.Parse(MessageArray[MessageArray.Length - 1]);
        PlayerControlThree target;
        PlayerControlThree Opposite;
        int playernumber = int.Parse(MessageArray[0]);
        if (playernumber.Equals(this.playerNumber))
        {
            target = ManagerHandler.Instance.GameManager().GetPlayer();
            Opposite = ManagerHandler.Instance.GameManager().GetEnemy();
        }
        else
        {
            target = ManagerHandler.Instance.GameManager().GetEnemy();
            Opposite = ManagerHandler.Instance.GameManager().GetPlayer();
        }

        WaitUntil messageWait = new WaitUntil(() => targetTime <= ManagerHandler.Instance.GameTime().GetTimePass());
        yield return messageWait;

        switch (messageType)
        {
            case "Seed":
                KingGodClient.Instance.SetSeed(messageSplit[1]);
                AudioManager.Instance.InitBackGroundAudio();
                AudioManager.Instance.InitEffectAudio();
                StartCoroutine(PlayManage.Instance.LoadScene("YangChigi4.0"));
                break;
            case "Ready":

                break;
            case "Start":
                StartCoroutine(ManagerHandler.Instance.GameUIManager().ReadyScreen());
                break;
            case "GameEnd":
                StartCoroutine(ManagerHandler.Instance.GameUIManager().GoToResultScene());
                break;
            case "Shepherd":
                if (playernumber.Equals(this.playerNumber))
                {
                    ManagerHandler.Instance.GameUIManager().GetPhaseButton().ChangeSearchButtonText(target.GetPlayerSearchState());
                }
                target.SendMessage("SearchPhaseShift");
                break;
            case "Skill":
                Vector3 skillVector = new Vector3(float.Parse(MessageArray[2]), float.Parse(MessageArray[3]), float.Parse(MessageArray[4]));
                StartCoroutine(SendMessageToSkillUse(int.Parse(MessageArray[1]), target, Opposite.gameObject, target.HQ.gameObject, skillVector, float.Parse(MessageArray[5])));
                break;
            case "Out":
                target.SetPlayerState(PlayerSearchState.BACKTOHOME);
                break;
                
        }
    }

    private IEnumerator SendMessageToSkillUse(int num, PlayerControlThree Player, GameObject Enemy, GameObject HQ, Vector3 HV, float useTime)
    {
        Vector3 targetVector = HQ.transform.position - HV;
        float angle = Mathf.Atan2(targetVector.x, targetVector.z) * Mathf.Rad2Deg;
        yield return new WaitUntil(() => ManagerHandler.Instance.GameTime().GetTimePass() >= (useTime + delayTime));
        ManagerHandler.Instance.SkillManager().UsingSkill(num, Player, Enemy, ManagerHandler.Instance.GameManager().GetPlanetTransform(), HQ.gameObject.transform, angle, HV);
    }

    private void FixedUpdate()
    {
        if (messageQueue.Count > 0)
        {
            StartCoroutine(MessageActor(messageQueue.Dequeue()));
        }
    }
    
}
