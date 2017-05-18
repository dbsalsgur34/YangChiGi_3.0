using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientSide;

public class GameUIManager : MonoBehaviour {

    public static GameUIManager GUIMInstance;

    private Text UItext;
    private Text UIscore;
    private Text UIcurrentSheep;
    private Text UIEnemyScore;
    private GameObject EndScreen;
    private Text EndText;

    private float SheepCount;
    private float initialtime;

    //Timer 관련 변수.
    private bool TimerStart;
    private float Timer = 0;
    private float startTime = 0;
    

    //플레이어
    private PlayerControlThree player;
    private PlayerControlThree enemy;

    private void Awake()
    {
        //UI관련 초기화.
        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIscore = GameObject.Find("ScoreText").GetComponent<Text>();
        UIcurrentSheep = GameObject.Find("CurrentSheepText").GetComponent<Text>();
        UIEnemyScore = GameObject.Find("EnemyScoreText").GetComponent<Text>();

        EndScreen = GameObject.Find("EndScreen");
        EndScreen.SetActive(true);
        EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.gameObject.SetActive(false);

        SheepCount = 0;

        TimerStart = false;
        initialtime = 100f;
    }

    private void Start()
    {
        GUIMInstance = this.gameObject.GetComponent<GameUIManager>();
        player = GameManager.GMInstance.GetPlayer();
        enemy = GameManager.GMInstance.GetEnemy();
    }

    public float GetTimePass()
    {
        return (Timer - startTime);
    }

    public bool GetIsTimerStart()
    {
        return TimerStart;
    }

    private void Showremainingtime()
    {
        string timetext;
        if (initialtime - Timer >= 0)
        {

            timetext = "Left Time : " + (initialtime - Timer).ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            if (TimerStart)
            {
                StartCoroutine(FinishRoutine());
            }
        }
        UItext.text = timetext;
    }

    private void ShowScore()
    {
        string scoretext;
        float PlayerScore = player.Score;
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
        SheepCount = player.SheepCount;
        if (SheepCount >= 10)
        {
            scoretext = "Current Sheep : " + SheepCount;
        }
        else
        {
            scoretext = "Current Sheep : 0" + SheepCount;
        }
        UIcurrentSheep.text = scoretext;
    }

    private void ShowEnemyScore()
    {
        string scoretext;
        float EnemyScore = enemy.Score;
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

    private void ShowUIText()
    {
        Showremainingtime();
        ShowScore();
        ShowEnemyScore();
        ShowMySheep();
    }

    public IEnumerator ReadyScreen()
    {
        player.GetPlayerState().IsStop = true;
        enemy.GetPlayerState().IsStop = true;
        EndText.gameObject.SetActive(true);
        EndText.text = "Ready...";
        yield return new WaitForSeconds(3f);
        //KingGodClient.Instance.GetNetworkMessageSender().SendStartedToServer();
        startTime = 0;
        EndScreen.SetActive(false);
        player.GetPlayerState().IsStop = false;
        enemy.GetPlayerState().IsStop = false;
        this.TimerStart = true;
        yield return 0;
    }

    private IEnumerator FinishRoutine()
    {
        EndScreen.SetActive(true);
        Text EndText = GameObject.Find("EndText").GetComponent<Text>();
        EndText.text = "Time Over!";
        player.GetPlayerState().IsStop = true;
        enemy.GetPlayerState().IsStop = true;
        PlayManage.Instance.PlayerScore = player.Score;
        PlayManage.Instance.EnemyScore = enemy.Score;
        KingGodClient.Instance.GetNetworkMessageSender().SendGameOverToServer(KingGodClient.Instance.playerNum ,GetTimePass());
        this.TimerStart = false;
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator GoToResultScene()
    {
        if (!this.GetIsTimerStart())
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayManage.Instance.LoadScene("Result"));
        }
    }

    private void CalTime()
    {
        if(TimerStart)
            Timer += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        ShowUIText();
        CalTime();
    }
}
