using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreText : LabeledUpdatableText
{
 

    // Update is called once per frame
    void Update()
    {
        setText(Game.instance.playerScore.ToString());
    }
}
