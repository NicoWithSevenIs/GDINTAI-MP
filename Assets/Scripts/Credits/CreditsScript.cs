using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public static bool isFirstTimeReading = true;
    public static int creditsRead = 0;

    private Dialogue dialogue;
    private bool isShown;

    [SerializeField] private KeyCode exitKey;
    [SerializeField] private string headerText;
    [TextArea(3, 10)]
    [SerializeField] private string displayText;

    //bandaid fix
    private bool skipUpdateThisFrame = false;

    public void Start()
    {
        dialogue = GetComponent<Dialogue>();
        isShown = false;
        UIManager.instance.creditsNextText.text = "Press [" + exitKey.ToString() +  "] to continue";
    }

    public void showCredits()
    {
        UIManager.instance.creditsHeaderText.text= headerText;
        UIManager.instance.creditsBodyText.text= displayText;

        UIManager.instance.creditsPanel.SetActive(true);
        Time.timeScale = 0f;
        isShown= true;
        skipUpdateThisFrame = true;
  
        creditsRead++;
    }

    private void Update()
    {
       
        if (!isShown)
            return;

        if(skipUpdateThisFrame)
        {
            skipUpdateThisFrame= false;
            return;
        }

        if (Input.GetKeyDown(exitKey))
        {
            UIManager.instance.creditsPanel.SetActive(false);
            Time.timeScale = 1f;
            isShown = false;
  
            if (isFirstTimeReading && dialogue != null)
            {
                dialogue.InitiateDialogue();
                isFirstTimeReading = false;
            }
      
        }
    }
}
