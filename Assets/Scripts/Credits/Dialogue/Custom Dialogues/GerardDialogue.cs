using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerardDialogue : ConditionalDialogue
{
    public override bool willTriggerAlternative()
    {
        return ChestScript.hasOpenedChest;
    }
}
