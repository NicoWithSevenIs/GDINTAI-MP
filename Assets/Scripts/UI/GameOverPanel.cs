using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : TweenableUI
{

    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Sound")]
    private GameObject soundOrigin;
    [SerializeField] private AudioClip winJingle;
    [SerializeField] private AudioClip loseJingle;

    private void Start()
    {
        Game.instance.onGameOver += InvokeGameOver;

        soundOrigin = new GameObject();
        soundOrigin.transform.position = Vector3.zero;

        AudioManager.instance.addSFX(winJingle, soundOrigin, 500, false);
        AudioManager.instance.addSFX(loseJingle, soundOrigin, 500, false);
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
                AudioManager.instance.PlaySFX(AudioManager.getName(winJingle, soundOrigin));
                break;
            case "Enemy":
                message = "Enemy Wins...";
                AudioManager.instance.PlaySFX(AudioManager.getName(loseJingle, soundOrigin));
                break;
            default: 
                message = "Draw!";
                break;
        }

        gameOverText.text = message;
        this.StartTween();

    }
}
