using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatableText : MonoBehaviour
{

    protected TextMeshProUGUI textGUI;
        
    private void Awake()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    public virtual void setText(string text)
    {
        this.textGUI.text = text;
    }

  
}
