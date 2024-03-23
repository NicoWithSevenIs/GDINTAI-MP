using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : PowerUp
{


    private void OnTriggerEnter2D(Collider2D collision)
    {


        List<GameObject> bases = collision.tag == "Player" ? Game.instance.playerBases : collision.tag == "Enemy" ? Game.instance.enemyBases : new List<GameObject> { };

        if (bases.Count == 0)
            return;

        foreach(var b in bases){
            b.GetComponent<Base>()?.setInvincible(10.0f);
        }

        Consume();
        
    }
}
