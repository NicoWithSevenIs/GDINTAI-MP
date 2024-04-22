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
        AudioManager.instance.addSFX(effectSFX, gameObject, 8, false);
    }
    protected virtual void Consume()
    {
        onConsumption?.Invoke();

        StartCoroutine(playSFX());
        gameObject.SetActive(false);
    }

    private IEnumerator playSFX()
    {
        print("Started");
        AudioManager.instance.PlaySFX(AudioManager.getName(consumeSFX, gameObject));
        yield return new WaitForSeconds(consumeSFX.length);
        print("Next");
        AudioManager.instance.PlaySFX(AudioManager.getName(effectSFX, gameObject));
    }


}
