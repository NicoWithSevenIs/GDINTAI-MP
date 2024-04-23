using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Input Hint")]
    public GameObject inputHint;
    public TextMeshProUGUI inputHintText;

    [Header("Dialogue Box")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueSpeakerName;
    public TextMeshProUGUI dialogueBoxText;
    public TextMeshProUGUI dialogueNextText;
    #region singleton
    public static UIManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);


    }
    #endregion
}
