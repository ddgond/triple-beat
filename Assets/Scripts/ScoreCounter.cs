using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private TextMeshPro tmp;
    private float score;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        ResetScore();
    }

    public void AddToScore(float points)
    {
        SetScore(score + points);
    }

    void ResetScore()
    {
        SetScore(0);
    }

    void SetScore(float newScore)
    {
        score = newScore;
        tmp.text = scoreAsString();
        ResultsInfo.score = scoreAsInt();
    }

    private int scoreAsInt()
    {
        return (int)Mathf.Round(score);
    }

    private string scoreAsString()
    {
        string scoreText = scoreAsInt().ToString();
        while (scoreText.Length < 7)
        {
            scoreText = "0" + scoreText;
        }
        return scoreText;
    }
}
