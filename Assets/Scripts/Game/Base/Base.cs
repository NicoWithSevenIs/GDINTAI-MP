using System;
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

    [Header("Base Texture")]
    [SerializeField] private Sprite activeTexture;
    [SerializeField] private Sprite destroyedTexture;


    [SerializeField] private GameObject[] InvincibilityObjects;

    [ColorUsageAttribute(true, false)]
    [SerializeField] private Color baseInvincibilityColor;
    public Timer invincibilityTimer { get; private set; }

    public event Action onBaseDestroyed;

    [Header("Game Logic")]
    [SerializeField] private string[] destroyOnTouch;
    public bool isDestroyed { get; private set; } = false;
    public bool isInvincible { get; private set; } = false;


    private SpriteRenderer sprite;

    #region positionHandling
        
    public void moveBase(Vector2 newPos)
    {

        if(this.isInvincible)
            foreach(var v in InvincibilityObjects)
            {
                v.GetComponent<IInvincibilityComponent>().Disable();
            }
          

        transform.position = newPos;

        if (this.isInvincible)
            foreach (var v in InvincibilityObjects)
            {
                v.SetActive(true);
            }
    }

    #endregion

    #region invincibility

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        foreach (var i in InvincibilityObjects)
        {
            i.SetActive(false);
        }
        invincibilityTimer = new Timer(1);

        invincibilityTimer.onElapse += () => {

            isInvincible = false;
            setColor(Color.white);
            foreach (var i in InvincibilityObjects)
            {
                IInvincibilityComponent inv = i.GetComponent<IInvincibilityComponent>();
                if (inv != null)
                    inv.Disable();
            }
        };

        invincibilityTimer.onStart += () =>
        {
            foreach (var i in InvincibilityObjects)
            {
                i.SetActive(true);
            }
        };

        sprite.sprite = activeTexture;
       
    }

    private void Update()
    {
        invincibilityTimer.TickDown(Time.deltaTime);
    }
    public void setInvincible(float duration)
    {

        if (isDestroyed)
            return;

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
        onBaseDestroyed?.Invoke();
       
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
        sprite.sprite = isDestroyed ? destroyedTexture : activeTexture;

    }
    #endregion collisionHandling
}
