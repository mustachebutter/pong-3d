using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private struct RoundData
    {
        public bool bIsPlayer1WonLastRound;
        public bool bHasLaunchedBall;
        public bool bIsNextLevel;
        public int playerScore;
        public int aiScore;
        public int roundNumber;
        public int level;
        public RoundData(int roundNumber_, int level_)
        {
            bIsPlayer1WonLastRound = false;
            bHasLaunchedBall = false;
            bIsNextLevel = false;
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

    void Awake()
    {
        Debug.Log("<color=red>Awake</color>");
        if (Instance != null)
        {
            Debug.Log($"<color=green>{Instance.roundData.level}</color>");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log($"<color=green>{Instance.roundData.level}</color>");
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("<color=red>Start</color>");
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
        if (roundData.bIsNextLevel)
        {
            LoadNextLevel();
        }
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
        if (roundData.playerScore == 1)
        {
            Debug.Log("You win!");
            // Load next level
            roundData.level++;
            roundData.bIsNextLevel = true;
        }
        else if (roundData.aiScore == WINNING_SCORE)
        {
            Debug.Log ("You lost :(");
        }

        roundData.roundNumber++;
        StartCoroutine(DelayTimer(5.0f, OnStartRound));
    }
    
    IEnumerator DelayTimer(float seconds, Action method)
    {
        Debug.Log("Starting delay timer");
        yield return new WaitForSecondsRealtime(seconds);

        Debug.Log("Finished delaying");
        method();
    }
    public void LoadNextLevel()
    {
        LoadNextScene();
        roundData.aiScore = 0;
        roundData.playerScore = 0;
        roundData.bIsNextLevel = false;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

}
