using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float lifetimeDuration = 2f;
    private Timer lifetime;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    public Vector2 direction;

 

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        direction = new Vector2(10, 0);
    }

    private void OnEnable()
    {
        lifetime = new Timer(lifetimeDuration);
        lifetime.startTimer();
        anim.SetBool("hasExploded", false);
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
        
        Vector2 velocity = direction;
        print(direction);
        velocity *= speed * Time.deltaTime;


        body.velocity = velocity;

    }

    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == gameObject.tag)
           return;

       collision.GetComponent<IDamageable>()?.TakeDamage(50);
       anim.SetBool("hasExploded", true);
       direction = Vector2.zero;
    }

    public void disable()
    {
       gameObject.SetActive(false);
    }

}
