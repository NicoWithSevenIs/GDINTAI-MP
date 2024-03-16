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

 

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();


    }

    private void OnEnable()
    {
        lifetime = new Timer(lifetimeDuration);
        lifetime.startTimer();
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
        
        Vector2 velocity = Vector2.zero;

        direction.Normalize();

        velocity.x =  10 * speed * Time.fixedDeltaTime;
        velocity.y = body.velocity.y;

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

        IDamageable damageable = collision.GetComponent<IDamageable>();

        damageable?.TakeDamage(50);

    }
    
}
