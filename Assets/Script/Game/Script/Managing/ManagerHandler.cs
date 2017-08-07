using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHandler : MonoBehaviour
{
    public static ManagerHandler Instance;

    private GameManager gameManager;
    private GameUIManager gameUIManager;
    private GameControlManager gameControlManager;
    private GameTime gameTime;
    private SkillManager skillManager;
    private NetworkManager networkManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitManager());   
    }

    private IEnumerator InitManager()
    {
        yield return new WaitUntil(() => gameManager != null);
        IEnumerator c = (gameManager.StartManagerInit());
        WaitUntil wait = new WaitUntil(() => c.MoveNext() == false);
        StartCoroutine(c);
        yield return wait;
        Debug.Log("gameManagerInit");
        c = gameTime.StartManagerInit();
        StartCoroutine(c);
        yield return wait;
        Debug.Log("gameTimeInit");
        c = gameUIManager.StartManagerInit();
        StartCoroutine(c);
        yield return wait;
        Debug.Log("gameUIManagerInit");
        c = gameControlManager.StartManagerInit();
        StartCoroutine(c);
        yield return wait;
        c = skillManager.StartManagerInit();
        StartCoroutine(c);
        yield return wait;
        c = networkManager.StartManagerInit();
        StartCoroutine(c);
        yield return wait;
    }

    public void SetManager(GameManager managerInstance)
    {
        gameManager = managerInstance;
    }
    public void SetManager(GameUIManager managerInstance)
    {
        gameUIManager = managerInstance;
    }
    public void SetManager(GameTime managerInstance)
    {
        gameTime = managerInstance;
    }
    public void SetManager(SkillManager managerInstance)
    {
        skillManager = managerInstance;
    }
    public void SetManager(GameControlManager managerInstance)
    {
        gameControlManager = managerInstance;
    }

    public void SetManager(NetworkManager managerInstance)
    {
        networkManager = managerInstance;
    }

    public GameManager GameManager()
    {
        return gameManager;
    }
    public GameUIManager GameUIManager()
    {
        return gameUIManager;
    }
    public GameControlManager GameControlManager()
    {
        return gameControlManager;
    }
    public GameTime GameTime()
    {
        return gameTime;
    }
    public SkillManager SkillManager()
    {
        return skillManager;
    }

    public NetworkManager NetworkManager()
    {
        return networkManager;
    }

}

