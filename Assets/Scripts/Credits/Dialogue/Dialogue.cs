using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    [SerializeField] private DialogueContainer dialogue;


    public void InitiateDialogue()
    {
        if (dialogue != null)
            DialogueManager.instance.StartDialogue(dialogue);
    }



}
