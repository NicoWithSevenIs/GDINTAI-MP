using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;

    [Header("Animator Parameters")]

    [SerializeField] private string deathTrigger;


    public event Action onRevive;
    public event Action onDie;
    public event Action<string> onHandleBlame;

    private Animator anim;
    private BoxCollider2D box;

    void Start()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        onDie += Death;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        onRevive?.Invoke();

        if(box != null )
        {
            box.enabled = true;
        }
        
    }



    public void TakeDamage(string origin, float damage)
    {
        currentHealth -= (int)damage;

        if (currentHealth <= 0)
        {
            onDie?.Invoke();
            onHandleBlame?.Invoke(origin);
        }

    }

    public void Death()
    {
        anim.SetTrigger(deathTrigger);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        box.enabled= false;
    }



}
