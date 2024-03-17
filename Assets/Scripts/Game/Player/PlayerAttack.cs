using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private Animator anim;
   
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

        onAttack += () => { anim.SetTrigger("isAttacking"); };
        onAttack += attackCooldownTimer.startTimer;
    }

    private void Update()
    {
        attackCooldownTimer.setDuration(attackCooldown);
        attackCooldownTimer.TickDown(Time.deltaTime);


        if (Input.GetMouseButtonDown(0) && !attackCooldownTimer.isTimerRunning)
        {
            onAttack?.Invoke();
        }    
    }

    public void Attack()
    {
       
        GameObject projectile = getFromPool();
        projectile.transform.position = attackOrigin.transform.position;
        projectile.SetActive(true);

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
        print("run");
        GameObject poolable = Instantiate(projectile);
        poolable.SetActive(false);
        poolable.transform.parent = projectileContainer.transform;
        ObjectPool.Add(poolable);

        return poolable;
    }

}
