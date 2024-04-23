using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> dialogueQueue;
    private bool isDisplayingText;

    [SerializeField] private KeyCode nextKey;
    private string currentMessage;

    //bandaid fix for now
    bool canSkip = false;

    [SerializeField] private float textSpeed = 0.1f;

    private void Start()
    {
        dialogueQueue= new Queue<string>();
        isDisplayingText = false;

        UIManager.instance.dialogueNextText.text = "Press [" + nextKey.ToString() + "] to continue...";
    }

    public void StartDialogue(DialogueContainer dialogue)
    {
    
        dialogueQueue.Clear();
        canSkip = false;

        foreach(var d in dialogue.dialogueText)
        {
            dialogueQueue.Enqueue(d);
        }

        UIManager.instance.dialogueSpeakerName.text = dialogue.name;
        UIManager.instance.dialogueBox.SetActive(true);
        Time.timeScale = 0f;
   
        next();

    }

    public void next()
    {

        if(dialogueQueue.Count == 0)
        {
            UIManager.instance.dialogueBox.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        currentMessage = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(displayText(currentMessage));

    }

    private IEnumerator displayText(string message)
    {
        isDisplayingText = true;
        UIManager.instance.dialogueBoxText.text = "";

        foreach(var c in message.ToCharArray())
        {
            UIManager.instance.dialogueBoxText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
            canSkip = true;
        }
        isDisplayingText = false;
    }


    private void Update()
    {


        if (Input.GetKeyDown(nextKey) && canSkip)
        {

            if (isDisplayingText)
            {
                StopAllCoroutines();
                isDisplayingText= false;
                UIManager.instance.dialogueBoxText.text = currentMessage;

            }
            else
            {
                next();
            }

        }
    }

    #region singleton

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }


    #endregion
}
