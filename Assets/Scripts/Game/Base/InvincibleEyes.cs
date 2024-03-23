using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEyes : MonoBehaviour, IInvincibilityComponent
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {  
        anim.SetBool("isExpunged", false);
    }

   
    public void OnExplosion()
    {
        gameObject.SetActive(false);
    }
     

    public void Disable()
    {
        anim.SetBool("isExpunged", true);
    }






}
