using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private struct RoundData
    {
        public bool bIsPlayer1WonLastRound;
        public bool bHasLaunchedBall;
        public int playerScore;
        public int aiScore;
        public int roundNumber;
        public int level;
        public RoundData(int roundNumber_, int level_)
        {
            bIsPlayer1WonLastRound = false;
            bHasLaunchedBall = false;
            playerScore = 0;
            aiScore = 0;
            roundNumber = roundNumber_;
            level = level_;
        }
    }
    private bool bIsRoundWon;
    public GameObject paddlePlayer;
    public GameObject paddleAI;
    public Ball ball;
    public ScoreTrigger player1ScoreTrigger;
    public ScoreTrigger player2ScoreTrigger;
    public UIManager uiManager;
    public const int WINNING_SCORE = 5;
    private RoundData roundData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roundData = new RoundData(roundNumber_: 1, level_: 1);
        player1ScoreTrigger.OnScoreTriggered += () =>
        {
            uiManager.SetGeneralText("PLAYER 1 SCORED");
            uiManager.SetScore(true, ++roundData.playerScore);
            roundData.bIsPlayer1WonLastRound = true;
            Time.timeScale = 0;
            OnEndRound();
        };

        player2ScoreTrigger.OnScoreTriggered += () =>
        {
            uiManager.SetGeneralText("PLAYER 2 SCORED");
            uiManager.SetScore(false, ++roundData.aiScore);
            roundData.bIsPlayer1WonLastRound = false;
            Time.timeScale = 0;
            OnEndRound();
        };

        OnStartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartRound()
    {
        Debug.Log("START ROUND");
        Time.timeScale = 1.0f;
        ball.ResetBall();
        player1ScoreTrigger.ResetTrigger();
        player2ScoreTrigger.ResetTrigger();  
        StartCoroutine(DelayTimer(3.0f, LaunchBall));
    }

    public void LaunchBall()
    {
        if (!roundData.bHasLaunchedBall)
        {
            Debug.Log("LAUNCH!");
            ball.Launch(roundData.bIsPlayer1WonLastRound);
            roundData.bHasLaunchedBall = true;
        }
    }
    
    public void OnEndRound()
    {
        if (roundData.playerScore == WINNING_SCORE)
        // if (roundData.playerScore == 1)
        {
            Debug.Log("You win!");
        }
        else if (roundData.aiScore == WINNING_SCORE)
        {
            Debug.Log ("You lost :(");
        }
        else
        {
            roundData.roundNumber++;
            if (roundData.roundNumber == 5)
            {
                roundData.level++;
            }
            StartCoroutine(DelayTimer(5.0f, OnStartRound));
        }
    }
    
    IEnumerator DelayTimer(float seconds, Action method)
    {
        Debug.Log("Starting delay timer");
        yield return new WaitForSecondsRealtime(seconds);

        Debug.Log("Finished delaying");
        method();
    }
}
