using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos : PowerUp
{

    private Timer transformTimer;
    private Animator anim;
 
    //I know this is inefficient and expensive down the line but i really dont feel like editing assets rn shjsdskaksdkk
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void nextColor()
    {
        anim.SetInteger("ChangeColors", (anim.GetInteger("ChangeColors") + 1) % 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        List<GameObject> bases = collision.tag == "Player" ? Game.instance.playerBases : collision.tag == "Enemy" ? Game.instance.enemyBases : new List<GameObject> { };

        if (bases.Count == 0)
            return;

       
        Game.invalidTileChecker isTileInvalid = (int x, int y) =>
        {

            bool tileNonPath = TilemapManager.instance.tileMapTypes[x, y] != TileType.Path;
            bool tileHasBase = Game.instance.basePositions[x, y];
            bool tileHasPotion = PowerUpManager.instance.potionPosTracker[x, y];
            //bool tileHasPlayer = TilemapManager.instance.playerPos.x == x && TilemapManager.instance.playerPos.y == y;
            //bool tileHasEnemy = TilemapManager.instance.enemyPos.x == x && TilemapManager.instance.enemyPos.y == y;

            return tileNonPath || tileHasBase || tileHasPotion /*|| tileHasPlayer || tileHasEnemy*/;

        };

        Game.instance.RearrangeBases(collision.tag == "Player", bases, isTileInvalid); 
       

        Consume();
    }
}
