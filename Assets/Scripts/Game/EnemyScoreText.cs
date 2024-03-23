using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScoreText : LabeledUpdatableText
{
    // Update is called once per frame
    void Update()
    {
        setText(Game.instance.enemyScore.ToString());
    }
}
