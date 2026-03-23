using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Player1Score;
    [SerializeField] private TextMeshProUGUI Player2Score;
    [SerializeField] private TextMeshProUGUI GeneralText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(bool bIsPlayer1, int score)
    {
        if (bIsPlayer1)
        {
            Player1Score.SetText(score.ToString());
        }
        else
        {
            Player2Score.SetText(score.ToString());
            Debug.Log(Player2Score.text);
        }
    }

    public void SetGeneralText(string text)
    {
        GeneralText.SetText(text);
    }
}
