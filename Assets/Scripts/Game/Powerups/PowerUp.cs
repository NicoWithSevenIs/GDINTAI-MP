using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public event Action onConsumption;

    protected void Consume()
    {
        onConsumption?.Invoke();
        gameObject.SetActive(false);
    }

    
}
