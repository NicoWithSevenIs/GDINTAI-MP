using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject[] playerBases;
    public GameObject[] enemyBases;



    // Update is called once per frame
    void Update()
    {
        
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
