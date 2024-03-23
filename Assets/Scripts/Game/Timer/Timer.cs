using System;

public class Timer
{

    private float baseDuration;
    private float timeElapsed;
    public bool isTimerRunning { get; private set; }

    public event Action onElapse;
    public event Action onStart;

    public Timer(float baseDuration)
    {
        this.baseDuration = baseDuration;
        timeElapsed = 0f;
        isTimerRunning = false;
    }

    public void TickDown(float deltaTime)
    {
        if (!isTimerRunning)
            return;

        if (timeElapsed < baseDuration)
        {
            timeElapsed += deltaTime;
        }

        if (timeElapsed >= baseDuration)
        {
            stopTimer();
        }

    }

    public float getTimeRemaining()
    {
        return baseDuration - timeElapsed;
    }

    #region TimerUtilities
    public void resetTimer()
    {
        timeElapsed = 0;
    }

    public void restartTimer()
    {
        resetTimer();
        startTimer();
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
        timeElapsed = 0f;
        onElapse?.Invoke();
    }

    public void setDuration(float time)
    {
        this.baseDuration = time;
    }

    public void recalibrateTimer(float time)
    {
        this.baseDuration = time;
        this.resetTimer();
    }

    #endregion TimerUtilities



}
