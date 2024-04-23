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

    [Header("Credits Panel")]
    public GameObject creditsPanel;
    public TextMeshProUGUI creditsHeaderText;
    public TextMeshProUGUI creditsBodyText;
    public TextMeshProUGUI creditsNextText;

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
