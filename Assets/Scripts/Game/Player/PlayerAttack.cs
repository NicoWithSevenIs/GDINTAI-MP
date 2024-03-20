using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackOrigin;
    [SerializeField] private GameObject projectileContainer;
    [SerializeField] private Texture2D cursorOverride;

    

    [Header("Object Pooling")]
    [SerializeField] private GameObject projectile;
    private List<GameObject> ObjectPool;
    [SerializeField] private int poolSize = 1;


    [Header("Stats")]
    [SerializeField] private float attackCooldown = 1f;
    private Timer attackCooldownTimer;


    public event Action onAttack;
    public event Action onAttackComplete;

    private Animator anim;

    private bool canAttack = true;
   
    private void Awake()
    {
        ObjectPool = new List<GameObject>();
        for (int i =0; i < poolSize; i++)
        {
            insertToPool();
        }
    }


    private void Start()
    {
        Cursor.SetCursor(cursorOverride, new Vector2(cursorOverride.width/2, cursorOverride.height/2) , CursorMode.ForceSoftware);
        anim = GetComponent<Animator>();
        attackCooldownTimer = new Timer(attackCooldown);

        onAttack += () => { 
            anim.SetTrigger("isAttacking");
            attackCooldownTimer.startTimer();
            GetComponent<PlayerMovement>().setTurning(false);
        };
      
        onAttackComplete += () => { 
            GetComponent<PlayerMovement>().setTurning(true); 
        };

        GetComponent<Health>().onDie += () =>
        {
            anim.ResetTrigger("isAttacking");
            canAttack = false;
        };
    }

    private void OnEnable()
    {
        canAttack = true;
    }

    private void Update()
    {
        attackCooldownTimer.setDuration(attackCooldown);
        attackCooldownTimer.TickDown(Time.deltaTime);


        if (Input.GetMouseButtonDown(0) && !attackCooldownTimer.isTimerRunning && canAttack)
        {
            onAttack?.Invoke();
        }    
    }

    public void Attack()
    {

        GameObject projectile = getFromPool();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float unsignedX = Mathf.Abs(transform.localScale.x);

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1 * unsignedX, transform.localScale.y);
        }
        else if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector2(unsignedX, transform.localScale.y);
        }

        Vector2 aimDir = mousePos - attackOrigin.transform.position;
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
        foreach(var pool in ObjectPool)
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
