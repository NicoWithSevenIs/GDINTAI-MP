using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRewardsDialogue : Dialogue
{

    [Header("Reward Dialogue")]
    [SerializeField] private DialogueContainer rewardDialogue;
    [SerializeField] private DialogueContainer claimedRewardsDialogue;

    [Header("Reward List")]
    [SerializeField] private string[] rewardsList;

    protected bool hasClaimedRewards = false;

    protected virtual void Start()
    {
        rewardDialogue.onDialogueFinished += () =>
        {
            foreach(var reward in rewardsList)
            {
                FauxInventoryScript.instance.AddItem(reward);
            }
        };
    }

    public virtual bool willGiveRewards()
    {
        return false;
    }

    public override void InitiateDialogue()
    {
        if(!hasClaimedRewards)
        {
            if (willGiveRewards())
            {
                DialogueManager.instance.StartDialogue(rewardDialogue);
                hasClaimedRewards = true;
            }
            else
            {
                base.InitiateDialogue();
            }
        }
        else
        {
            DialogueManager.instance.StartDialogue(claimedRewardsDialogue);
        }



        

    }
}
