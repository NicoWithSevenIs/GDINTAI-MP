using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private GameObject projectile;
    

    [Header("Stats")]
    [SerializeField] private float attackCooldown = 1f;
    private Timer attackCooldownTimer;

    private Animator anim;
   
    private void Start()
    {
        anim = GetComponent<Animator>();
        attackCooldownTimer = new Timer(attackCooldown);
    }

    private void Update()
    {
        attackCooldownTimer.setDuration(attackCooldown);
        attackCooldownTimer.TickDown(Time.deltaTime);


        if (Input.GetMouseButtonDown(0) && !attackCooldownTimer.isTimerRunning)
        {
            anim.SetTrigger("isAttacking");
            attackCooldownTimer.startTimer();
        }
            
    }





}
