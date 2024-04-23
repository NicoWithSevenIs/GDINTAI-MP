using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    [SerializeField] protected DialogueContainer dialogue;


    public virtual void InitiateDialogue()
    {
        if (dialogue != null)
            DialogueManager.instance.StartDialogue(dialogue);
    }



}
