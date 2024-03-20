using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{


    [Header("Statue Parts")]
    //bandage solution bc i dont wanna go animator with this
    [SerializeField] private GameObject statueTop;
    [SerializeField] private GameObject statueBottom;
    [SerializeField] private GameObject statueDestroyed;

    [ColorUsageAttribute(true, false)]
    [SerializeField] private Color baseInvincibilityColor;
    private Timer invincibilityTimer;


    [Header("Game Logic")]
    [SerializeField] private string[] destroyOnTouch;
    public bool isDestroyed { get; private set; } = false;
    private bool isInvincible = false;


    private SpriteRenderer sprite;

    #region invincibility

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        invincibilityTimer = new Timer(1);
        invincibilityTimer.onElapse += () => {
            isInvincible = false;
            setColor(Color.white); 
        };
    }

    private void Update()
    {
        invincibilityTimer.TickDown(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            setInvincible(1.5f);
        }

    }
    public void setInvincible(float duration)
    {
        isInvincible = true;
        setColor(baseInvincibilityColor);
        invincibilityTimer.recalibrateTimer(duration);
        invincibilityTimer.startTimer();
    }

    private void setColor(Color color)
    {
        if(isDestroyed) 
            return;
        sprite.color = color;
        statueTop.GetComponent<SpriteRenderer>().color = color;
        statueBottom.GetComponent<SpriteRenderer>().color = color;
    }

    #endregion

    #region collisionHandling
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isInvincible)
            return;

        bool getDestroyedOnTouch = false;
        foreach(var s in destroyOnTouch)
        {
            if(collision.tag == s)
                getDestroyedOnTouch = true;
        }
        if (!getDestroyedOnTouch)
            return;


        setStatueDestroyed(true);
       
    }

    private void OnEnable()
    {
        setStatueDestroyed(false);
        isInvincible = false;
    }

    public void setStatueDestroyed(bool isDestroyed)
    {
        this.isDestroyed = isDestroyed;
        statueTop.SetActive(!isDestroyed);
        statueBottom.SetActive(!isDestroyed);
        statueDestroyed.SetActive(isDestroyed);
    }
    #endregion collisionHandling
}
