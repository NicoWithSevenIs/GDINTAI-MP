using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{

    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private  KeyCode activationKey;
    [SerializeField] private string actionString;

    private bool canActivate;

    private void Update()
    {
        if(Input.GetKeyDown(activationKey) && canActivate)
        {
            onActivate?.Invoke();
            setActivatable(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            UIManager.instance.inputHintText.text = "[" + activationKey.ToString() + "] - " + actionString;
            setActivatable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            setActivatable(false);
        }
    }

    private void setActivatable(bool willActivate)
    {
        canActivate = willActivate;
        UIManager.instance.inputHint.SetActive(willActivate);
    }

}
