using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SophoclesRewards : HiddenRewardsDialogue
{
    public override bool willGiveRewards()
    {
        return CreditsScript.creditsRead >= 3;
    }
}
