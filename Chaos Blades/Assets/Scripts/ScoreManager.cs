using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //this object only exist while the game is running and when we show the statics screen (game over screen),
    //once the game ends and players restart the level, this object should be deleted and the new object should take charge,
    //and hence the scores are reseted. 

    //if want to give players more stats, i.e. how many different type of enemies killed and what different spells used,
    //can store all in an array and hope it works


    public int currentScore;
    public float timeSurvived;


    #region Singleton Code
    public static ScoreManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this); 
        else
            instance = this; 
    }
    #endregion

    private void Start()
    {
        StartGame();
    }

    //code is called when the game starts
    public void StartGame()
    {
        currentScore = 0;
        timeSurvived = 0;
    }

    //code is called when enemy dies
    public void EnemyKilled(int pointsEnemyGive)
    {
        currentScore += pointsEnemyGive;
    }

    //code is called when player dies
    public void GameOver()
    {
        //Time.time returns the time at current frame in seconds,
        //so need to modify it accordingly
        timeSurvived = Time.time;

        Debug.Log("Total Score: " + currentScore + "\nTime Taken: " + timeSurvived);
    }
}
