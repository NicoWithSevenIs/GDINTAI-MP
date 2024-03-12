using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabeledUpdatableText : UpdatableText
{

    [SerializeField] private string preText = "";
    [SerializeField] private string postText = "";

    public override void setText(string text)
    {
        this.textGUI.text = preText + text + postText;
    }
}
