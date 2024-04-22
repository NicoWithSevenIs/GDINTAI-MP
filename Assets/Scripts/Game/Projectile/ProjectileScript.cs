using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private string ownerTag;
    [SerializeField] private string[] blackList;
    [SerializeField] private float lifetimeDuration = 2f;

    [Header("Animator Parameters")]
    [SerializeField] private string detonateTrigger;

    private Timer lifetime;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    public Vector2 direction;
    public Vector2 currentDirection;


    bool isFirstEnabled = false;

    [Header("Sounds")]
    [SerializeField] private AudioClip onProjectileFire;
    [SerializeField] private AudioClip onProjectileHit;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        direction = new Vector2(10, 0);
        currentDirection = direction;

    }

    private void OnEnable()
    {
        lifetime = new Timer(lifetimeDuration);
        lifetime.startTimer();
        anim.SetBool(detonateTrigger, false);
        currentDirection = direction;

        if(!isFirstEnabled)
        {
            AudioManager.instance.addSFX(onProjectileHit, gameObject, 16, false);
            AudioManager.instance.addSFX(onProjectileFire, gameObject, 16, false);
            isFirstEnabled = true;
        }
      
        AudioManager.instance.PlaySFX(AudioManager.getName(onProjectileFire, gameObject));
    }

    private void Update()
    {
        lifetime.TickDown(Time.deltaTime);

        if(!lifetime.isTimerRunning)
        {
            gameObject.SetActive(false);
            direction = Vector2.zero;
        }

    }

    private void FixedUpdate()
    {
        
        Vector2 velocity = currentDirection;
        velocity.Normalize();
        body.velocity = velocity * speed;

    }

    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ownerTag || collision.isTrigger)
            return;

        AudioManager.instance.PlaySFX(AudioManager.getName(onProjectileHit, gameObject));
        foreach (string s in blackList)
        {
            if (collision.tag == s)
                return;
        }

       collision.GetComponent<IDamageable>()?.TakeDamage(ownerTag, 50);
       anim.SetBool(detonateTrigger, true);
       currentDirection = Vector2.zero;
    }

    public void disable()
    {
       gameObject.SetActive(false);
    }

}
