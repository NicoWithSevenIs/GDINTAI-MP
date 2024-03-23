using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : PowerUp
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(1);
            Consume();
        }
    
    }

}
