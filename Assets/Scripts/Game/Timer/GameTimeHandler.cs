using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeHandler : MonoBehaviour
{

    [SerializeField] private float baseDuration;
    public Timer timer { get; private set; }

    private void Start()
    {
        timer = new Timer(baseDuration);
        timer.startTimer();
    }


    private void Update()
    {
        timer.TickDown(Time.deltaTime);
    }

    #region singleton
    public static GameTimeHandler instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
    #endregion singleton

}
