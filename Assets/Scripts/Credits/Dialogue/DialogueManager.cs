using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> dialogueQueue;
    private bool isDisplayingText;

    [SerializeField] private KeyCode nextKey;
    private string currentMessage;
    public event Action onDialogueFinished;

    public event Action onDialogueBeginInitializers;
    public event Action onDialogueFinishedInitializers;

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
        onDialogueBeginInitializers?.Invoke();
        dialogueQueue.Clear();
        canSkip = false;

        foreach(var d in dialogue.dialogueText)
        {
            dialogueQueue.Enqueue(d);
        }

        UIManager.instance.dialogueSpeakerName.text = dialogue.name;
        UIManager.instance.dialogueBox.SetActive(true);
        Time.timeScale = 0f;

        onDialogueFinished += dialogue.onDialogueFinished;
   
        next();

    }

    public void next()
    {

        if(dialogueQueue.Count == 0)
        {
            UIManager.instance.dialogueBox.SetActive(false);
            Time.timeScale = 1f;
            onDialogueFinished?.Invoke();
            onDialogueFinished = null;
            onDialogueFinishedInitializers?.Invoke();
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

        if (!UIManager.instance.dialogueBox.activeInHierarchy)
            return;

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
