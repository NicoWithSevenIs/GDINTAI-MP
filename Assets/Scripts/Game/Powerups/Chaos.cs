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
        Dictionary<string, List<GameObject>> r = new Dictionary<string, List<GameObject>>()
        {
            {"Player", Game.instance.playerBases},
            {"Enemy", Game.instance.enemyBases}
        };

        if (!r.TryGetValue(collision.tag, out List<GameObject> a))
            return;


        Consume();
    }
}
