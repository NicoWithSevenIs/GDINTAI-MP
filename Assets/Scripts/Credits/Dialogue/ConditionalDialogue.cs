using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDialogue : Dialogue
{
    [SerializeField] protected DialogueContainer alternativeDialogue;

    public virtual bool willTriggerAlternative()
    {
        return false;
    }

    public override void InitiateDialogue()
    {

        if(willTriggerAlternative())
        {
            DialogueManager.instance.StartDialogue(alternativeDialogue);
        }
        else
        {
            base.InitiateDialogue();
        }
        
    }
}
