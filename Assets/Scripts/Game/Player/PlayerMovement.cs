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

        velocity.x = HorizontalInput * Time.deltaTime;
        velocity.y = VerticalInput * Time.deltaTime;

        velocity.Normalize();
        velocity *= speed;

        body.velocity = velocity;
    }

    private void flipSprite()
    {
        if (HorizontalInput < 0f)
            sprite.flipX = true;
        else if (HorizontalInput > 0f)
            sprite.flipX = false;
    }
}
