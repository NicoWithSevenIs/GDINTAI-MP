using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemGiver : Dialogue
{
    [SerializeField] private string itemName;
    [SerializeField] private DialogueContainer alreadyRewardedDialogue;

    private void Start()
    {
        dialogue.onDialogueFinished += () =>
        {
            FauxInventoryScript.instance.AddItem(itemName);
        };
    }

    public override void InitiateDialogue()
    {
        if(FauxInventoryScript.instance.isItemObtained(itemName))
        {
            DialogueManager.instance.StartDialogue(alreadyRewardedDialogue);
        }
        else
        {
            base.InitiateDialogue();
        }
        
    }

}
