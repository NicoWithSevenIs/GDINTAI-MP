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


    public event Action onDie;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        onDie += Death;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;

        if(currentHealth <= 0 )
        {
            onDie?.Invoke();
        }
    }

    public void Death()
    {
        anim.SetTrigger(deathTrigger);
        StartCoroutine(delayedDisable());
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    //temp
    public IEnumerator delayedDisable()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

}
