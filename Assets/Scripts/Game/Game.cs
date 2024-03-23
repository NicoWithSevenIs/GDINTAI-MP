using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Bases")]
    public GameObject[] playerBases;
    public GameObject[] enemyBases;

   

    [Header("Game Duration")]
    [SerializeField] private float gameDuration = 121f;
    public Timer gameTimer { get; private set; }

    public int playerScore { get; private set; }
    public int enemyScore { get; private set; }


    public event Action onGameOver;

    private void Start()
    {

        gameTimer = new Timer(gameDuration);
        gameTimer.startTimer();

        gameTimer.onElapse += () =>
        {
            //Check Win con here
        };

   


        initializeBase(true, playerBases);
        initializeBase(false, enemyBases);

        SpawnBases();

    }

    //Call this in chaos powerup
    private void SpawnBases()
    {

    }

    private void initializeBase(bool isPlayer, GameObject[] bases)
    {
        foreach(var b in bases)
        {
            var baseScript = b.GetComponent<Base>();
            
            if(baseScript == null)
                continue;

            baseScript.onBaseDestroyed += () => {

                if (isPlayer)   
                    enemyScore += 100;
                else playerScore += 100;

                //Check here if any agent has run out of bases

            };
        }
    }

    private bool hasAgentLost(GameObject[] bases)
    {
        int count = 0;

        foreach(var b in bases)
        {
            var baseScript = GetComponent<Base>();
            if (baseScript == null)
                continue;

            if (!baseScript.isDestroyed)
                count++;
        }

        return count == 0;
    }
   
    private void Update()
    {
        gameTimer.TickDown(Time.deltaTime);
    }

    #region singleton
    public static Game instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
