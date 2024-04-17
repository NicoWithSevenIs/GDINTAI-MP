using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().fillAmount = ((float)PlayerPrefs.GetInt("Difficulty") + 1) / 3f;
    }

 
}
