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
	

	private State currentState;
	private GameObject currentTarget;

    void Start()
    {
        anim = GetComponent<Animator>();
        //onCompleteAction += updateStateMachine;
		enemyMovement = GetComponent<EnemyMovement>();
        //enemyMovement.setTarget(player);


		setState(State.BaseHunting);
    }

	

    void Update()
    {
        Vector3Int cellPos = TilemapManager.instance.WorldToCell(transform.position);
        switch (currentState)
		{
			default:
			case State.BaseHunting:

			
				if(!currentTarget || currentTarget.GetComponent<Base>().isDestroyed)
				{
                    currentTarget = Game.instance.getBaseNearestTo(cellPos, true);
                    if (currentTarget)
                        enemyMovement.setTarget(currentTarget);
				}


			
			

				break;
		}
    }


	void setState(State state)
	{

		Vector3Int cellPos = TilemapManager.instance.WorldToCell(transform.position);

		switch (state)
		{
			default:
			case State.BaseHunting:
				currentTarget = Game.instance.getBaseNearestTo(cellPos, true);
				if (currentTarget)
					enemyMovement.setTarget(currentTarget);
				break;
		}
	}


    private void updateStateMachine()
    {
        float cellSightRange = Mathf.Pow(PlayerPrefs.GetInt("Difficulty") + 1, 2);
        

    }
}


