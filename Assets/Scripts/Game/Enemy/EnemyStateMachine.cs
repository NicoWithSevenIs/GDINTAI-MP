using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D body;


    private Pathfinding pathfinder;
    private List<Node> waypoints;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        updateStateMachine();

        anim.SetBool("isWalking", body.velocity != Vector2.zero);
    }

    private void updateStateMachine()
    {   
        
    }
}
