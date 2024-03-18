using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float speed;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sprite;

    private float HorizontalInput = 0;
    private float VerticalInput = 0;

    private bool canTurn = true;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        anim.SetBool("isWalking", HorizontalInput != 0 || VerticalInput != 0);

        flipSprite();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 velocity = Vector2.zero;

        velocity.x = HorizontalInput * Time.fixedDeltaTime;
        velocity.y = VerticalInput * Time.fixedDeltaTime;

        velocity.Normalize();
        velocity *= speed;

        body.velocity = velocity;
    }

    private void flipSprite()
    {

        if (!canTurn)
            return;

        float unsignedX = Mathf.Abs(transform.localScale.x);
        if (HorizontalInput < 0f)
            transform.localScale = new Vector2 (-1 * unsignedX, transform.localScale.y);
        else if(HorizontalInput > 0f)
            transform.localScale = new Vector2(unsignedX, transform.localScale.y);
    }   

    public void setTurning(bool canTurn)
    {
        this.canTurn= canTurn;
    }
}
