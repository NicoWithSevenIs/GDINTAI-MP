using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;


//ermm this and player attack should have a common superclass to inherit from but in the interest of time i'll just ermm do this
public class EnemyAttack : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject attackOrigin;
    [SerializeField] private GameObject projectileContainer;

    [Header("Object Pooling")]
    [SerializeField] private GameObject projectile;
    private List<GameObject> ObjectPool;
    [SerializeField] private int poolSize = 1;


    [Header("Stats")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown = 1f;
    private Timer attackCooldownTimer;

    public event Action onAttack;
    public event Action onAttackComplete;

    private Animator anim;

    private bool canAttack = true;

    private void Awake()
    {
        ObjectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            insertToPool();
        }
    }


    private void Start()
    {
        anim = GetComponent<Animator>();
        attackCooldownTimer = new Timer(attackCooldown);

        onAttack += () => {
            anim.SetTrigger("isEnemyAttacking");
            attackCooldownTimer.startTimer();
        };

        onAttackComplete += () => {
            
        };

        GetComponent<Health>().onDie += () =>
        {
            anim.ResetTrigger("isEnemyAttacking");
            canAttack = false;
        };
    }

    private void OnEnable()
    {
        canAttack = true;
    }

    private void Update()
    {
        Debug.DrawRay(attackOrigin.transform.position, player.transform.position-attackOrigin.transform.position);

        attackCooldownTimer.setDuration(attackCooldown);
        attackCooldownTimer.TickDown(Time.deltaTime);

        if (attackCooldownTimer.isTimerRunning || !canAttack)
            return;

       
        if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
           

            RaycastHit2D r = Physics2D.Raycast(attackOrigin.transform.position, player.transform.position-attackOrigin.transform.position, attackRange);

            print(r.collider.gameObject.name);  
            if (r && r.collider.gameObject == player)
            {
                onAttack?.Invoke();
            }
        }
        
        
        
    }

    public void Attack()
    {

        GameObject projectile = getFromPool();

        Vector3 playerPos = player.transform.position;

        float unsignedX = Mathf.Abs(transform.localScale.x);


        if (playerPos.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1 * unsignedX, transform.localScale.y);
        }
        else if (playerPos.x > transform.position.x)
        {
            transform.localScale = new Vector2(unsignedX, transform.localScale.y);
        }

        Vector2 aimDir = playerPos - attackOrigin.transform.position;
        projectile.GetComponent<ProjectileScript>().setDirection(aimDir);
        projectile.transform.position = attackOrigin.transform.position;
        projectile.SetActive(true);

    }

    public void AttackCompleted()
    {
        onAttackComplete?.Invoke();
    }

    private GameObject getFromPool()
    {
        foreach (var pool in ObjectPool)
        {
            if (!pool.activeInHierarchy)
            {
                return pool;
            }
        }

        return insertToPool();
    }

    private GameObject insertToPool()
    {
        GameObject poolable = Instantiate(projectile);
        poolable.SetActive(false);
        poolable.transform.parent = projectileContainer.transform;
        ObjectPool.Add(poolable);

        return poolable;
    }

}
