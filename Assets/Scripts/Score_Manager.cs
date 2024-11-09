using UnityEngine;
using TMPro;

public class Score_Manager : MonoBehaviour
{
    public static Score_Manager Instance; 
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text streakText; // New text for displaying the streak

    private int currentScore = 0;
    public int highScore = 0;
    private int currentStreak = 0; 
    private int streakBonus = 5; // Bonus points for consecutive hits

    private void Awake()
    {
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

    public void AddScore(int points, bool isConsecutiveHit) 
    {
        currentScore += points;

        if (isConsecutiveHit) 
        {
            currentStreak++;
            currentScore += streakBonus; // Add bonus points for streak
            streakText.text = "Streak: " + currentStreak;
        }
        else
        {
            currentStreak = 0; // Reset streak
            streakText.text = "Streak: 0"; 
        }

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

    public int GetCurrentScore()
    {
        return currentScore;
    }
}