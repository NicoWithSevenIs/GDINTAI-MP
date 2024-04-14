using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
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

                
                //If the player's bases are invincible, fight the player instead
                if (Game.instance.areBasesInvincible(true))
                    currentState = State.Dueling;
                else 
                /*
                    If the player's close to a base and the agent's bases are not invincible and
                    if the base is not deliberately ignored, switch to defend mode
                */
                if (willDefendBase(nearestEnemyBaseToPlayer) && !Game.instance.areBasesInvincible(false))
                {                  
                    currentState = State.Defending;
                }
                else
                /*
                    If the player's nearby and the agent's NOT in the ignore player proximity,
                    the distance from the agent and their target range where the agent instead
                    makes a beeline for the base instead of dueling the player, duel the player
                 */
                if (!isPlayerEliminated && isPlayerInSight() && !willIgnorePlayer())
                {
                    currentState = State.Dueling;
                }
                    
                
               
               
                break;

			case State.Defending:

                Vector3Int eCell = TilemapManager.instance.WorldToCell(transform.position);
                Vector2Int eIndex = TilemapManager.instance.CellToIndex(new Vector2Int(eCell.x, eCell.y));
                BoundsInt bounds = TilemapManager.instance.maxBoundsData.Value;

                GameObject nearestInvincible = null;
                GameObject nearestChaos = null;


                //find the nearest Invincibility and Chaos potions cellSeekRange away from the agent's current tile
                for(int i = Mathf.Max(0, eIndex.y - cellSeekRange); i <= Mathf.Min(bounds.size.y-1, eIndex.y + cellSeekRange ); i++)
                {
                    for (int j = Mathf.Max(0, eIndex.x - cellSeekRange); j <= Mathf.Min(bounds.size.x-1, eIndex.x + cellSeekRange); j++)
                    {
                        Vector2Int cell = TilemapManager.instance.IndexToCell(j, i);
                        GameObject potAtCell = PowerUpManager.instance.getPowerUpAt(cell);


                        //Debug.DrawLine(transform.position, TilemapManager.instance.CellToWorld(cell));

                        if (potAtCell == null)
                            continue;


                       
                        /*
                        Action<GameObject> setNearest = (GameObject nearest) =>
                        {
                            if(nearest == null)
                            {
                                nearest = potAtCell;
                            }
                            else
                            {
                                float distFromNearest = Vector3.Distance(transform.position, nearest.transform.position);
                                float distFromCurrent = Vector3.Distance(transform.position, potAtCell.transform.position);

                                if (distFromCurrent < distFromNearest)
                                    nearest = potAtCell;
                            }
                        };
                        */

                        switch (potAtCell.name)
                        {
                            case "Invincibility":
                                if (nearestInvincible == null)
                                {
                                    nearestInvincible = potAtCell;
                                }
                                else
                                {
                                    float distFromNearest = Vector3.Distance(transform.position, nearestInvincible.transform.position);
                                    float distFromCurrent = Vector3.Distance(transform.position, potAtCell.transform.position);

                                    if (distFromCurrent < distFromNearest)
                                        nearestInvincible = potAtCell;
                                }
                                //setNearest(nearestInvincible);
                                break;
                            case "Chaos":
                                if (nearestChaos == null)
                                {
                                    nearestChaos = potAtCell;
                                }
                                else
                                {
                                    float distFromNearest = Vector3.Distance(transform.position, nearestChaos.transform.position);
                                    float distFromCurrent = Vector3.Distance(transform.position, potAtCell.transform.position);

                                    if (distFromCurrent < distFromNearest)
                                        nearestChaos = potAtCell;
                                }
                                break;

                        }

                     
                    }

                }

                if(nearestChaos)
                    Debug.DrawLine(transform.position, nearestChaos.transform.position, Color.magenta);

                if (nearestInvincible)
                    Debug.DrawLine(transform.position, nearestInvincible.transform.position, Color.blue);
                


                //if player is away from base or if the base being defended has been destoyed or the base is now invincible
                if (!willDefendBase(nearestEnemyBaseToPlayer) || nearestEnemyBaseToPlayer.GetComponent<Base>().isDestroyed || Game.instance.areBasesInvincible(false))
                {   
                    //go back to base hunting
                    currentState = State.BaseHunting;

                }
                else
                //If player is within attackRange, duel with them instead
                if (!isPlayerEliminated && isPlayerInSight())
                {
                    currentState = State.Dueling;
                }
                else
                {   
                      
                    float playerDistFromBase = Vector3.Distance(nearestEnemyBaseToPlayer.transform.position, player.transform.position);

                    if(!nearestInvincible && !nearestChaos || playerDistFromBase > defendRange)
                    {   
                        currentState = State.BaseHunting;
                    }
                    else if (nearestInvincible  && Vector3.Distance(transform.position, nearestInvincible.transform.position) < playerDistFromBase)
                    {
                        enemyMovement.setTarget(nearestInvincible);
                    }
                    else if (nearestChaos && Vector3.Distance(transform.position, nearestChaos.transform.position) < playerDistFromBase)
                    {
                        enemyMovement.setTarget(nearestChaos);
                    }
                        

                }

                break;


            case State.Dueling:
                enemyMovement.setTarget(player);

                float playerDist = Vector3.Distance(transform.position, player.transform.position);

               
                if(willDefendBase(nearestEnemyBaseToPlayer) && !Game.instance.areBasesInvincible(false) && playerDist > aggroRange)
                {
                    currentState = State.Defending;
                }
                else if (!Game.instance.areBasesInvincible(true) && (isPlayerEliminated || willIgnorePlayer() || playerDist > aggroRange))
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


