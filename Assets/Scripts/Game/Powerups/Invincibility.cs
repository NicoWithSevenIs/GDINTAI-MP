using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        GameObject[] bases = collision.tag == "Player" ? Game.instance.playerBases : collision.tag == "Enemy" ? Game.instance.enemyBases : new GameObject[] { };

        if (bases.Length == 0)
            return;

        foreach(var b in bases){
            b.GetComponent<Base>()?.setInvincible(10.0f);
        }
        
        gameObject.SetActive(false);
    }
}
