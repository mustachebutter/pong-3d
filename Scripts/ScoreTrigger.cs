using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool bIsTriggered = false;
    public delegate void ScoreHandler();
    public event ScoreHandler OnScoreTriggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball" && !bIsTriggered)
        {
            bIsTriggered = true;
            OnScoreTriggered?.Invoke();
        }
    }

    public void ResetTrigger()
    {
        bIsTriggered = false;
    }
}
