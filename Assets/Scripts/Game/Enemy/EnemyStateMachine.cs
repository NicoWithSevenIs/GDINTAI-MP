using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    BaseHunting, //Base Hunting by default
	Dueling, //
	Defending, //If player 

}

//The enemy will always fire at the player if they're nearby 

public class EnemyStateMachine : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject player;



    //Components
    private Animator anim;
    private Rigidbody2D body;
	private EnemyMovement enemyMovement;


	private int difficultyFactor;
    private float defendRange;
    private float aggroRange;
    private int cellSeekRange;

    //if the agent is in the ignorePlayerProximity distance between it and its target base and, do not duel the player 
    private float ignorePlayerProximity;

	private State currentState;
	private GameObject currentTarget;

    private GameObject baseToDefend;

    private bool isPlayerEliminated = false;

    void Start()
    {
        anim = GetComponent<Animator>();
		enemyMovement = GetComponent<EnemyMovement>();

		difficultyFactor = PlayerPrefs.GetInt("Difficulty");

        defendRange = 4f + difficultyFactor * 1.5f;
        ignorePlayerProximity = 5f + difficultyFactor;
        aggroRange = 5f + difficultyFactor * 2;
        cellSeekRange = 1 + difficultyFactor * 2;

        Debug.Log("Defend Range:" + defendRange);
        Debug.Log("Ignore Range:" + ignorePlayerProximity);
        Debug.Log("Aggro Range:" + aggroRange);

        GetComponent<EnemyAttack>().setAttackRange(aggroRange);

        baseToDefend = null;


        player.GetComponent<Health>().onRevive += () =>
        {
            isPlayerEliminated = false;
        };

        player.GetComponent<Health>().onDie += () =>
        {
            isPlayerEliminated = true; 
        };

        setState(State.BaseHunting);
        
    }

	//check player mag using space
	
	

    void Update()
    {
        Vector3Int enemyCell = TilemapManager.instance.WorldToCell(transform.position);
        Vector3Int playerCell = TilemapManager.instance.WorldToCell(player.transform.position);

        GameObject nearestEnemyBaseToPlayer = Game.instance.getBaseNearestTo(playerCell, false);

        if (!nearestEnemyBaseToPlayer)
        {
            Debug.Log("No nearest base to player");
            return;
        }
            
      


        switch (currentState)
		{
			default:
			case State.BaseHunting:

		
                currentTarget = Game.instance.getBaseNearestTo(enemyCell, true);

                if (!currentTarget)
                {
                    return;
                }


                enemyMovement.setTarget(currentTarget);

                //Transition Checker for Defend

                if (willDefendBase(nearestEnemyBaseToPlayer))
                {
                    if (!Game.instance.areBasesInvincible(false))
                    {

                        currentState = State.Defending;
                        baseToDefend = nearestEnemyBaseToPlayer;
                        Debug.Log("defending base");
                    }
                }
                else
                {
                    /*
                    if (!isPlayerEliminated && isPlayerInSight() && !willIgnorePlayer())
                    {
                        currentState = State.Dueling;
                        Debug.Log("dueling player");
                    }
                    */
                }
               
               

                break;

			case State.Defending:

                if(baseToDefend)
                    enemyMovement.setTarget(baseToDefend);
                if (!willDefendBase(nearestEnemyBaseToPlayer) || baseToDefend.GetComponent<Base>().isDestroyed)
                {
                    currentState = State.BaseHunting;
                    baseToDefend = null;
                    Debug.Log("back to base hunting");

                }
                break;


            case State.Dueling:
                enemyMovement.setTarget(player);


               
                if (isPlayerEliminated || willIgnorePlayer())
                {
                    currentState = State.BaseHunting;
                }

                break;

		}

  
    }


    bool willIgnorePlayer()
    {
        return Vector3.Distance(currentTarget.transform.position, transform.position) < ignorePlayerProximity;
    }

    bool willDefendBase(GameObject toDefend)
    {
        if (!toDefend.GetComponent<Base>())
            return false;

        return Vector3.Distance(toDefend.transform.position, player.transform.position) < defendRange;
    }

    bool isPlayerInSight()
    {
        return Vector3.Distance(player.transform.position, transform.position) < aggroRange;
    }

	void setState(State state) { 
		this.currentState = state;
	}



}


