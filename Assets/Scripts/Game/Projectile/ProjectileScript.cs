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


    //temp
    public Vector2 direction;
    public Vector2 currentDirection;

 

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

        foreach(string s in blackList)
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
