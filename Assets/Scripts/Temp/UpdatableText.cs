using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatableText : MonoBehaviour
{
    private TextMeshProUGUI text;


    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

     
        List<string> difficulties = new List<string> { 
            "Easy", "Medium", "Hard"
        };

        text.text = "Difficulty: " + difficulties[PlayerPrefs.GetInt("Difficulty")];
    }


}
