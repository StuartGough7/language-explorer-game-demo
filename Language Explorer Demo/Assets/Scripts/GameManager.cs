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

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    bool gameOver = false;
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
            case PageState.GameOver:
                startPage.SetActive(false);
                countPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                countPage.SetActive(false);
                gameOverPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        // activated when player hits the replay button
    }

    public void StartGame()
    {
        // activated when player hits the play button
    }
}
