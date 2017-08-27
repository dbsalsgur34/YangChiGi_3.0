using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientSide;

public class GameUIManager : GameManagerBase {

    private Text UItext;
    private Text UIscore;
    private Text UIcurrentSheep;
    private Text UIEnemyScore;
    private GameObject EndScreen;
    private Text EndText;

    private PhaseShiftButton phaseButton;
    private float sheepCount;

    //플레이어
    private PlayerControlThree player;
    private PlayerControlThree enemy;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
        AudioManager.Instance.InitBackGroundAudio();
    }

    protected override void InitManager()
    {
        base.InitManager();
        //Awake에서 실행되던 것들
        //UI관련 초기화.
        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIscore = GameObject.Find("ScoreText").GetComponent<Text>();
        UIcurrentSheep = GameObject.Find("CurrentSheepText").GetComponent<Text>();
        UIEnemyScore = GameObject.Find("EnemyScoreText").GetComponent<Text>();
        phaseButton = GameObject.Find("PhaseButton").GetComponent<PhaseShiftButton>();
        EndScreen = GameObject.Find("EndScreen");
        EndScreen.SetActive(true);
        EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.gameObject.SetActive(false);
        sheepCount = 0;

        //Start에서 실행되던 것들

    }

    private void Showremainingtime()
    {
        string timetext;
        if (ManagerHandler.Instance.GameTime().GetRemainTime() >= 0)
        {
            timetext = "Left Time : " + ManagerHandler.Instance.GameTime().GetRemainTime().ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            if (GameTime.IsTimerStart())
            {
                StartCoroutine(FinishRoutine());
            }
        }
        UItext.text = timetext;
    }

    private void ShowScore(int PlayerScore)
    {
        string scoretext;
        if (PlayerScore >= 10)
        {
            scoretext = "My Score : " + PlayerScore;
        }
        else
        {
            scoretext = "My Score : 0" + PlayerScore;
        }
        UIscore.text = scoretext;
    }

    private void ShowMySheep()
    {
        string scoretext;
        sheepCount = player.GetHerdSheepControl().GetHerdSheepCount();
        if (sheepCount >= 10)
        {
            scoretext = "Current Sheep : " + sheepCount;
        }
        else
        {
            scoretext = "Current Sheep : 0" + sheepCount;
        }
        UIcurrentSheep.text = scoretext;
    }

    private void ShowEnemyScore(int EnemyScore)
    {
        string scoretext;
        if (EnemyScore >= 10)
        {
            scoretext = "Enemy Score : " + EnemyScore;
        }
        else
        {
            scoretext = "Enemy Score : 0" + EnemyScore;
        }
        UIEnemyScore.text = scoretext;
    }

    public IEnumerator ReadyScreen()
    {
        EndText.gameObject.SetActive(true);
        EndText.text = "Ready...";
        AudioManager.Instance.PlayOneShotEffectClipByName("Sheep_Bleating");
        yield return new WaitForSeconds(5f);
        AudioManager.Instance.PlayEffectClipByName("Whistle", 0f, 1f);
        //KingGodClient.Instance.GetNetworkMessageSender().SendStartedToServer();
        EndText.text = "GO!!!";
        yield return new WaitForSeconds(2f);
        EndScreen.SetActive(false);
        ManagerHandler.Instance.GameTime().StartTimer();
        player = ManagerHandler.Instance.GameManager().GetPlayer();
        enemy = ManagerHandler.Instance.GameManager().GetEnemy();
        AudioManager.Instance.InitEffectAudio();
        AudioManager.Instance.PlayBackGroundClipByName("Battle", 0f);
    }

    private IEnumerator FinishRoutine()
    {
        EndScreen.SetActive(true);
        Text EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.text = "Time Over!";
        AudioManager.Instance.InitBackGroundAudio();
        AudioManager.Instance.InitEffectAudio();
        AudioManager.Instance.PlayEffectClipByName("Whistle2", 0f, 3f);
        PlayManage.Instance.PlayerScore = player.HQ.GetHQHerd().GetHerdSheepCount();
        PlayManage.Instance.EnemyScore = enemy.HQ.GetHQHerd().GetHerdSheepCount();
        ManagerHandler.Instance.GameTime().StopTimer();
        yield return new WaitForSeconds(3f);
        Debug.Log("End");
        AudioManager.Instance.InitEffectAudio();
        ManagerHandler.Instance.NetworkManager().GetNetworkMessageSender().SendGameOver(KingGodClient.Instance.playerNum, ManagerHandler.Instance.GameTime().GetTimePass());
    }

    public IEnumerator GoToResultScene()
    {
        if (!GameTime.IsTimerStart())
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayManage.Instance.LoadScene("Result"));
        }
    }

    private void FixedUpdate()
    {
        if (GameTime.IsTimerStart())
        {
            Showremainingtime();
            ShowMySheep();
            ShowScore(player.HQ.GetHQHerd().GetHerdSheepCount());
            ShowEnemyScore(enemy.HQ.GetHQHerd().GetHerdSheepCount());
        }
    }

    public PhaseShiftButton GetPhaseButton()
    {
        return this.phaseButton;
    }
}
