using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public event Action onConsumption;

    [SerializeField] private AudioClip consumeSFX;
    [SerializeField] private AudioClip effectSFX;

    private void Start()
    {
        AudioManager.instance.addSFX(consumeSFX, gameObject, 8, false);
        AudioManager.instance.addSFX(effectSFX, gameObject, 500, false);
    }
    protected virtual void Consume()
    {
        onConsumption?.Invoke();
        AudioManager.instance.PlaySFXSequential(new List<string>
        {
            AudioManager.getName(consumeSFX, gameObject),
            AudioManager.getName(effectSFX, gameObject)
        }, 0f);

        gameObject.SetActive(false);
    }



}
