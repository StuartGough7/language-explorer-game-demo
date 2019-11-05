using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour
{
    Text highScore;

    private void OnEnable() // we use this instead of start beacus we need to re check the high score every time the screen shows not just on mount
    {
        highScore = GetComponent<Text>();
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        
    }

}
