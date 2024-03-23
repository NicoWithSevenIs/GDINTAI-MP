using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerText : LabeledUpdatableText
{

    private void Update()
    {
        float TimeRemaining = Game.instance.gameTimer.getTimeRemaining();

        int minute = (int)(TimeRemaining / 60);
        int seconds = (int)(TimeRemaining % 60);

        string minuteText = itosmindd(minute);
        string secondsText = itosmindd(seconds);


        setText(minuteText + " : " + secondsText);
    }

    //int to string min double digit
    private string itosmindd(int number)
    {
        //Negative Handling
        Mathf.Abs(number);

        //If a number is at least triple digit, just return its string
        if (number > 99)
            return $"{number}";

        return (number < 10 ? "0" : "") + $"{number}";
    }
  
 
}
