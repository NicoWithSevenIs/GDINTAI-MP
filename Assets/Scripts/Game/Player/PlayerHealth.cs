using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;

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
        anim?.SetBool("isDead", false);
        currentHealth = maxHealth;
        foreach (var i in GetComponents<MonoBehaviour>())
        {
            i.enabled = true;
        }

    }


    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;

        if(currentHealth <= 0 )
        {
            print("Died");
            onDie?.Invoke();
        }
    }

    public void Death()
    {
        anim.SetBool("isDead", true);

        foreach(var i in GetComponents<MonoBehaviour>())
        {
            if(i != this)
                i.enabled = false;
        }

        print("clean up in 3 seconds");
        StartCoroutine(delayedDisable());
    }

    //temp
    public IEnumerator delayedDisable()
    {
        yield return new WaitForSeconds(3);
        print("died, cleaning up");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(1);
        }
    }


}
