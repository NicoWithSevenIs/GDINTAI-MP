using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueContainer 
{

    public string name;

    [TextArea(1,3)]
    public string[] dialogueText;

    public Action onDialogueFinished;
    
}
