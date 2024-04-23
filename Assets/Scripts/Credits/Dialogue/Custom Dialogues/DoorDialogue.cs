using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorDialogue : ConditionalDialogue
{

    public void Start()
    {

        dialogue.onDialogueFinished += () =>
        {
            SceneManager.LoadScene("MainMenu");
        };

        alternativeDialogue.onDialogueFinished += () =>
        {
            SceneManager.LoadScene("MainMenu");
        };
        
    }
    public override bool willTriggerAlternative()
    {
        return !FauxInventoryScript.instance.isInventoryEmpty();
    }
}
