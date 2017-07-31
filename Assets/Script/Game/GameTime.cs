using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : GameManagerBase {

	private static bool timerStart;
    private float timer;
    private float startTime;
    private float initialtime;

    protected override void Start () {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
    }

    protected override void InitManager()
    {
        base.InitManager();
        //Start에서 실행되던 것들
        timerStart = false;
        initialtime = 10f;
        startTime = 0;
        timer = 0;
    }

    public void StartTimer()
    {
        timerStart = true;
    }

    public void StopTimer()
    {
        timerStart = false;
    }

    public static bool IsTimerStart()
    {
        return timerStart;
    }
    
    public float GetTimePass()
    {
        return (timer - startTime);
    }

    public float GetRemainTime()
    {
        return (initialtime - timer);
    }

    public void Reset()
	{
		startTime = Time.time;
	}

    private void CalTime()
    {
        if (timerStart)
            timer += Time.deltaTime;
    }

    public float GetTime()
	{
		return Time.time - startTime; 
	}

    private void FixedUpdate()
    {
        CalTime();
    }


}
