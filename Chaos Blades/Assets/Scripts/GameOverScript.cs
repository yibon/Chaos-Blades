using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    private void Start()
    {
        int minutes = Mathf.FloorToInt(ScoreManager.instance.timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(ScoreManager.instance.timeSurvived - minutes * 60);

        //ScoreManager.instance.GameOver();
        scoreText.text = "Total Score: " + ScoreManager.instance.currentScore + "\nTime Taken: " + string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
