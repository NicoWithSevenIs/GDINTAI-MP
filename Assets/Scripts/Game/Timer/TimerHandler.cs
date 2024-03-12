using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{

    [SerializeField] private float baseDuration;
    private float timeElapsed;
    private bool isTimerRunning;


    public event Action onElapse;
    public event Action onStart;

   
    private void Start()
    {
        timeElapsed = 0;
        isTimerRunning = true;
    }


    private void Update()
    {
        if (!isTimerRunning)
            return;

        if(timeElapsed < baseDuration)
        {
            timeElapsed += Time.deltaTime;
        }

        if(timeElapsed >= baseDuration)
        {
            stopTimer();
        }
    }

    public float  getTimeRemaining()
    {
        return baseDuration - timeElapsed;
    }

    //temp
    private void OnDestroy()
    {
        timeElapsed = 0;
        isTimerRunning = false;
    }

    #region TimerUtilities
    public void resetTimer()
    {
        timeElapsed = 0;
    }

    public void restartTimer()
    {
        resetTimer();
        onStart.Invoke();
    }

    public void startTimer()
    {
        isTimerRunning = true;
        onStart?.Invoke();
    }

    
    public void pauseTimer()
    {
        isTimerRunning = false;
    
    }

    public void resumeTimer()
    {
        if (timeElapsed > 0)
            return;

        isTimerRunning = true;
    }

    public void stopTimer()
    {
        isTimerRunning = false;
        onElapse?.Invoke();
    }
    #endregion TimerUtilities
    #region auxillary

    //Read Only in the inspector Bandage Fix I dont feel like learning how to make it [ReadOnly]
    [SerializeField] public float TimeElapsed { get { return timeElapsed; } }
    #endregion
    #region singleton
    public static TimerHandler instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
    #endregion singleton
}
