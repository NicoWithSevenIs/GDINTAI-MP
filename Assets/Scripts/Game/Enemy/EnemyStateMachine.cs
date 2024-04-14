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


//if player is nearby and they're not near a base and the agent is not near the target base, duel the player
//The enemy will always fire at the player if they're nearby 

public class EnemyStateMachine : MonoBehaviour
{
	//Components
    private Animator anim;
    private Rigidbody2D body;

	private Vector2Int currentTarget;

	private State currentState;


    private event Action onCompleteAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        onCompleteAction += updateStateMachine;
    }

  
    void Update()
    {
		
    }

    private void updateStateMachine()
    {
        float cellSightRange = Mathf.Pow(PlayerPrefs.GetInt("Difficulty") + 1, 2);
        

    }
}


/*
 

sunday tasks

finish by 6pm tops
- brainstorm statemachine //somewhat ok
- implement state machine
- fix bug: potion and base repositions should spawn on top of any agent.
- stage design

others

- improve ui
- improve vfx
- gameplay qol: bases should have a minimum distance from other spawned bases


//test heuristics

state machine




factors: 

amount of player bases
amount of agent's bases
player's position
potion position

agent by default paths towads the nearest base



If the agent spots an object of interest,  evaluate next action

	if player (prioritized)
		if agent's bases > player bases, 
			throw attacks while the player is within sight
		if the player is near one of the bases
			check environment
			if there are potions nearby the distance between the agent and the potion is less than the player,
				path towards the potion
			otherwise 
				chase the player
		otherwise
			throw attacks while the player is within sight

	if potion
		if mine avoid it (set heuristic in aStar to math.inf)
		if agent's bases > player bases
			ignore and continue pathing towards the base	
		
		
		
		if invincibility 
			if already invincible, ignore it
			if nearby, path towards it.
			if player is near a base, get the nearest invincible potion

		
		if chaos
			if nearby check if player is near one of the bases
				//if it would put the agent at a disadvantage, ignore it
				//if the player is near one of the bases and the distance between the agent and the potion is
					less than the distance between the player and the potion,
						path towards it.


				
On medium and hard, if the player is near a base do an area check and act accordingly











 
 
 */
