using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilitySigils : MonoBehaviour, IInvincibilityComponent
{
    private Vector3 originalPos;
    private SpriteRenderer sprite;


    [SerializeField] private Sprite[] possibleTextures;
    [SerializeField] private LeanTweenType easingStyle = LeanTweenType.easeInOutCubic;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalPos = transform.position;
    }


    private void OnEnable()
    {
        originalPos = transform.position;

        sprite.sprite = possibleTextures[Random.Range(0, possibleTextures.Length)];
        float offset = Random.Range(-0.3f, 0.3f);
        float duration = Random.Range(3f, 5f);

        LeanTween.moveY(gameObject, originalPos.y + offset, duration).setEase(easingStyle).setLoopPingPong();
    }

    private void OnDisable()
    {
        LeanTween.cancelAll();
        transform.position = originalPos;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
