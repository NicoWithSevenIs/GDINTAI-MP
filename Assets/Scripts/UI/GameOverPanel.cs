using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : TweenableUI
{

    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Start()
    {
        Game.instance.onGameOver += InvokeGameOver;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void InvokeGameOver(string winner)
    {
        Time.timeScale = 0f;
        string message;
        switch(winner)
        {
            case "Player":
                message = "Player Wins!";
                break;
            case "Enemy":
                message = "Enemy Wins...";
                break;
            default: 
                message = "Draw!";
                break;
        }

        gameOverText.text = message;
        this.StartTween();

    }
}
