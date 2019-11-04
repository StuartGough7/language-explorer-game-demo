using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance; // This creates a singleton reference for other scripts to access all the public variables on this class
    public GameObject startPage;
    public GameObject countPage;
    public GameObject gameOverPage;
    public Text scoreText;

    int score = 0;
    bool gameOver = false;

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    private void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished; // creates a subscription to the CountdownText script and the Oncountdown event within that script
        TapController.OnPlayerScored += OnPlayerScored; // creates a subscription to the CountdownText script and the Oncountdown event within that script
        TapController.OnPlayerDied += OnPlayerDied; // creates a subscription to the CountdownText script and the Oncountdown event within that script
    }

    private void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished; // removes subscription
        TapController.OnPlayerScored -= OnPlayerScored; // removes subscription
        TapController.OnPlayerDied -= OnPlayerDied; // removes subscription
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted(); // event sent to TapConmtroller
        score = 0;
        gameOver = false;
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if(score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }

    public bool GameOver { get { return gameOver; } }
    private void Awake()
    {
        Instance = this; // ties the instance refernce to this class
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                countPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                countPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                countPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                countPage.SetActive(false);
                gameOverPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver() // activated when player hits the replay button
    {
        OnGameOverConfirmed(); // event sent to TapConmtroller
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame() // activated when player hits the play button
    {
        SetPageState(PageState.Countdown);
    }
}
