using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public int selectedDifficulty {get; private set;}


    public event Action<int> onMapSelect;
    public event Action<int> onDifficultySelect;


    //in case map names get updated down the line
    private List<string> mapNames= new List<string> { 
        "Map1",
        "Map2",
        "Map3"
    };


    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        PlayerPrefs.DeleteKey("Difficulty");

        selectDifficulty(0);
        setMap();

    }

    public void increaseDifficulty()
    {
        selectDifficulty(selectedDifficulty + 1);
    }

    public void decreaseDifficulty()
    {
        selectDifficulty(selectedDifficulty - 1);
    }

    private void selectDifficulty(int difficulty)
    {
        selectedDifficulty = Math.Clamp(difficulty, 0, 2);
        onDifficultySelect?.Invoke(selectedDifficulty);
    }

    public void setMap()
    {
        onMapSelect?.Invoke(PlayerPrefs.GetInt("SelectedMap"));
    }

    public void selectMap(int mapNum)
    {
        PlayerPrefs.SetInt("SelectedMap", Math.Clamp(mapNum, 0, mapNames.Count - 1));
        setMap();
    }

    public void Go()
    {   
        SceneManager.LoadScene(mapNames[PlayerPrefs.GetInt("SelectedMap")]);
        PlayerPrefs.SetInt("Difficulty", selectedDifficulty);
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    #region singleton
    public static MenuManager instance { get; private set; }
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else 
            Destroy(gameObject);

    }
    #endregion singleton

}
