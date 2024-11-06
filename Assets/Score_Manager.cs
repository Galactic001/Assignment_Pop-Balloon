using UnityEngine;
using TMPro;

public class Score_Manager : MonoBehaviour
{
    public static Score_Manager Instance; // Singleton instance
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadHighScore();
        UpdateHighScoreText();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreText();

        if (currentScore > highScore)
        {
            highScore = currentScore;
            UpdateHighScoreText();
            SaveHighScore();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}