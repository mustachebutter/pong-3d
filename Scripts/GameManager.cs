using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool bIsRoundWon;
    public GameObject paddlePlayer;
    public GameObject paddleAI;
    public GameObject ball;
    public GameObject playerScoreTrigger;
    public GameObject aiScoreTrigger;
    private int playerScore = 0;
    private int aiScore = 0;
    private bool bIsPlayerLastRoundWon = false;
    public const int WINNING_SCORE = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreTrigger playerST = playerScoreTrigger.GetComponent<ScoreTrigger>();
        ScoreTrigger aiST = aiScoreTrigger.GetComponent<ScoreTrigger>();

        playerST.OnScoreTriggered += () =>
        {
            Debug.Log("Player scored");
            Time.timeScale = 0;
            playerScore++;
            playerST.ResetTrigger();
        };
        aiST.OnScoreTriggered += () =>
        {
            Debug.Log("AI scored");
            Time.timeScale = 0;
            aiScore++;
            aiST.ResetTrigger();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnScore()
    {
        // if (playerScore == WINNING_SCORE)
        if (playerScore == 1)
        {
            Debug.Log("You win!");
        }
        else if (aiScore == WINNING_SCORE)
        {
            Debug.Log ("You lost :(");
        }
        else
        {
            // Reset Ball
            // Reset time scale
        }

        

    }

    
}
