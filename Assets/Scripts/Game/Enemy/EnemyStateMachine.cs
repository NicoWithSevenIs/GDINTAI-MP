using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStates
{
    Patrolling,
    Pathing,
    Attacking,
}

public class EnemyStateMachine : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D body;


    private Pathfinding pathfinder;
    private List<Node> waypoints;


    private event Action onCompleteAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        onCompleteAction += updateStateMachine;
    }

  
    void Update()
    {
        updateStateMachine();

        anim.SetBool("isWalking", body.velocity != Vector2.zero);
    }

    private void updateStateMachine()
    {
        float cellSightRange = Mathf.Pow(PlayerPrefs.GetInt("Difficulty") + 1, 2);
        

    }
}
