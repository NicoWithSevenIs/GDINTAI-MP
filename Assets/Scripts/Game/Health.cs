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
        
        if(box != null )
        {
            box.enabled = true;
        }
        
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
        box.enabled= false;
    }

    //temp
    public IEnumerator delayedDisable()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

}
