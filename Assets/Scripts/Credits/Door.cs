using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject exitHint;

    bool canExit = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canExit)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canExit = true;
            exitHint.SetActive(true);
        }
          
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            exitHint.SetActive(false);
            canExit = false;
        }
           
    }
}
