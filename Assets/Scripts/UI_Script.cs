using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public Button pauseButton;
    public Button resumeButton;
    public Button settingsButton; 
    public GameObject settingsPanel; // The panel for color settings
    public Image panelToColor;  // To change the color of Panel
    
    // GameOver panel
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverHighScoreText;
    public Image gameOverPanel;

    private bool isPaused = false;
    public string gameOverSceneName = "Main Menu";
    public Balloon_Spawner balloonSpawner;

    public Color[] colorOptions = new Color[3]; // Array to hold your color options

    void Start()
    {
        resumeButton.gameObject.SetActive(false); 
        settingsButton.gameObject.SetActive(false); 
        settingsPanel.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(ShowSettings);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (balloonSpawner.currentSpawnTime <= 0f && !isPaused) 
        {
            gameOverPanel.gameObject.SetActive(true);
            PauseGame(); // Pause the game
            
            // Update score and high score on the Game Over panel
            gameOverScoreText.text = "Score: " + Score_Manager.Instance.GetCurrentScore();
            gameOverHighScoreText.text = "High Score: " + Score_Manager.Instance.highScore; 
        }
    }

    public void PauseGame()
    {
        pauseButton.gameObject.SetActive(false); 
        resumeButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName); 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // New methods for the settings functionality
    public void ShowSettings()
    {
        settingsPanel.SetActive(true); 
    }

    public void SetColor(int colorIndex) 
    {
        if (colorIndex >= 0 && colorIndex < colorOptions.Length)
        {
            panelToColor.color = colorOptions[colorIndex];
            settingsPanel.SetActive(false); // Close the panel after selection
        }
        else
        {
            Debug.LogError("Invalid color index!");
        }
    }
}