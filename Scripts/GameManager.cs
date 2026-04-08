using System;
using System.Collections;
using System.Linq;
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
    public Paddle[] player1Paddles;
    public Paddle[] player2Paddles;
    public Ball ball;
    public ScoreTrigger[] player1ScoreTriggers;
    public ScoreTrigger[] player2ScoreTriggers;
    public UIManager uiManager;
    public const int WINNING_SCORE = 5;
    private RoundData roundData;
    private ScoreTrigger.ScoreHandler _p1ScoreHandler;
    private ScoreTrigger.ScoreHandler _p2ScoreHandler;

    void Awake()
    {
        Debug.Log("<color=red>Awake</color>");
        if (Instance != null)
        {
            Debug.Log($"<color=green>{Instance.roundData}</color>");
            foreach (Transform child in transform)
            {
                switch (child.name)
                {
                    case "Ball":
                        Instance.ball = child.GetComponent<Ball>();
                    break;
                    case "Player1Triggers":
                        Instance.player1ScoreTriggers = child.GetComponentsInChildren<ScoreTrigger>(true);
                    break;
                    case "Player2Triggers":
                        Instance.player2ScoreTriggers = child.GetComponentsInChildren<ScoreTrigger>(true);
                    break;
                    case "Player1Paddles":
                        Instance.player1Paddles = child.GetComponentsInChildren<Paddle>(true);
                    break;
                    case "Player2Paddles":
                        Instance.player2Paddles = child.GetComponentsInChildren<Paddle>(true);
                    break;
                }
            }

            foreach(Transform child in Instance.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                child.SetParent(Instance.transform);
            }
            Destroy(gameObject);
            Instance.OnInit();
            Instance.OnStartRound();
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        roundData = new RoundData(roundNumber_: 1, level_: 1);
        ball = transform.Find("Ball").GetComponent<Ball>();
        player1ScoreTriggers = transform.Find("Player1Triggers").GetComponentsInChildren<ScoreTrigger>(true);
        player2ScoreTriggers = transform.Find("Player2Triggers").GetComponentsInChildren<ScoreTrigger>(true);
        player1Paddles = transform.Find("Player1Paddles").GetComponentsInChildren<Paddle>(true);
        player2Paddles = transform.Find("Player2Paddles").GetComponentsInChildren<Paddle>(true);
        _p1ScoreHandler = () => HandlePlayerScoring(true);
        _p2ScoreHandler = () => HandlePlayerScoring(false);

        OnInit();
        OnStartRound();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInit()
    {
        Debug.Log("<color=red>Init</color>");
        foreach (var trigger in Instance.player1ScoreTriggers)
        {
            trigger.OnScoreTriggered -= _p1ScoreHandler;
            trigger.OnScoreTriggered += _p1ScoreHandler;
        }

        foreach (var trigger in Instance.player2ScoreTriggers)
        {
            trigger.OnScoreTriggered -= _p2ScoreHandler;
            trigger.OnScoreTriggered += _p2ScoreHandler;
        }
    }

    private void HandlePlayerScoring(bool bIsPlayer1)
    {        
        if (bIsPlayer1)
        {
            uiManager.SetGeneralText("PLAYER 1 SCORED");
            uiManager.SetScore(bIsPlayer1, ++roundData.playerScore);
            roundData.bIsPlayer1WonLastRound = true;
        }
        else
        {
            uiManager.SetGeneralText("PLAYER 2 SCORED");
            uiManager.SetScore(bIsPlayer1, ++roundData.aiScore);
            roundData.bIsPlayer1WonLastRound = false;
        }
        
        Time.timeScale = 0;
        OnEndRound();
    }

    public void OnStartRound()
    {
        Debug.Log("<color=red>Start</color>");
        if (Instance.roundData.bIsNextLevel)
        {
            Instance.LoadNextLevel();
        }
        
        Time.timeScale = 1.0f;
        Instance.ball.ResetBall();
        Instance.player1ScoreTriggers.ToList<ScoreTrigger>().ForEach(t => t.ResetTrigger());
        Instance.player2ScoreTriggers.ToList<ScoreTrigger>().ForEach(t => t.ResetTrigger());
        StartCoroutine(Instance.DelayTimer(3.0f, Instance.LaunchBall));
    }

    public void LaunchBall()
    {
        if (!Instance.roundData.bHasLaunchedBall)
        {
            Debug.Log("LAUNCH!");
            ball.Launch(Instance.roundData.bIsPlayer1WonLastRound);
            Instance.roundData.bHasLaunchedBall = true;
        }
    }
    
    public void OnEndRound()
    {
        // if (Instance.roundData.playerScore == WINNING_SCORE)
        if (Instance.roundData.playerScore == 1)
        {
            Debug.Log("You win!");
            // Load next level
            Instance.roundData.level++;
            Instance.roundData.bIsNextLevel = true;
        }
        else if (Instance.roundData.aiScore == WINNING_SCORE)
        {
            Debug.Log ("You lost :(");
        }

        Instance.roundData.roundNumber++;
        StartCoroutine(Instance.DelayTimer(5.0f, Instance.OnStartRound));
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
        Instance.roundData.aiScore = 0;
        Instance.roundData.playerScore = 0;
        Instance.roundData.bIsNextLevel = false;
        Instance.roundData.bHasLaunchedBall = false;
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
