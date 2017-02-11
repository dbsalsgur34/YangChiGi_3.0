using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : ManagerBase{

    float playerScore;
    float enemyScore;
    float EXP;

    Text playerName;
    Text enemyName;
    Text playerresult;
    Text enemyresult;

    GameObject win;
    GameObject lose;
    Image FadeImage;
    Text EXPgaintext;
    GameObject Back_To_Lobby;

	// Use this for initialization
	void Start ()
    {
        StartInit();
        StartCoroutine("ResultRoutine");
	}

    void StartInit()
    {
        playerScore = PlayManage.Instance.PlayerScore;
        enemyScore = PlayManage.Instance.EnemyScore;
        EXP = PlayManage.Instance.EXP;

        playerName = GameObject.Find("PlayerName").GetComponent<Text>();
        enemyName = GameObject.Find("EnemyName").GetComponent<Text>();
        playerresult = GameObject.Find("PlayerScore").GetComponent<Text>();
        enemyresult = GameObject.Find("EnemyScore").GetComponent<Text>();
        win = GameObject.Find("Win");
        lose = GameObject.Find("Lose");
        EXPgaintext = GameObject.Find("EXPText").GetComponent<Text>();
        FadeImage = GameObject.FindWithTag("Fadescreen").GetComponent<Image>();
        Back_To_Lobby = GameObject.Find("Back_To_Lobby");

        playerName.text = PlayManage.Instance.playerID;
        

        playerName.gameObject.SetActive(false);
        enemyName.gameObject.SetActive(false);
        playerresult.gameObject.SetActive(false);
        enemyresult.gameObject.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        EXPgaintext.gameObject.SetActive(false);
        Back_To_Lobby.SetActive(false);
    }

    IEnumerator ShowScore(Text scoreowner, Text scoretext,float targetscore)
    {
        scoreowner.gameObject.SetActive(true);
        scoretext.gameObject.SetActive(true);
        for (int i = 0; i <= targetscore; i++)
        {
            scoretext.text = "Score : " + i.ToString("N0");
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator ResultRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        IEnumerator playerroutine = ShowScore(playerName, playerresult, playerScore);
        StartCoroutine(playerroutine);
        yield return new WaitUntil(() => playerroutine.MoveNext() == false);
        yield return new WaitForSeconds(0.5f);

        IEnumerator enemyroutine = ShowScore(enemyName, enemyresult, enemyScore);
        StartCoroutine(enemyroutine);
        yield return new WaitUntil(() => enemyroutine.MoveNext() == false);
        yield return new WaitForSeconds(1f);

        if (playerScore > enemyScore)
        {
            GameWin();
        }
        else if (playerScore < enemyScore)
        {
            GameLose();
        }
        else
        {
            GameTie();
        }
        yield return new WaitForSeconds(1f);
        EXPgaintext.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        Back_To_Lobby.SetActive(true);
#if UNITY_EDITOR
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
#elif UNITY_ANDROID
        yield return new WaitUntil(() => Input.GetTouch(0).tapCount > 0);
#endif
        StartCoroutine(PlayManage.Instance.LoadScene("Lobby"));
    }

    void GameWin()
    {
        win.SetActive(true);
        EXPgaintext.text = "EXP + 100";
        PlayManage.Instance.EXP += 100;
    }

    void GameLose()
    {
        lose.SetActive(true);
        EXPgaintext.text = "EXP + 10";
        PlayManage.Instance.EXP += 10;
    }

    void GameTie()
    {
        EXPgaintext.text = "EXP + 25";
        PlayManage.Instance.EXP += 25;
    }
}
