using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Script : MonoBehaviour
{
    // public GameObject pauseMenuUI;
    public Button pauseButton;
    public Button resumeButton;

    private bool isPaused = false;

    public string gameOverSceneName = "MainMenu";

    // Scripts Reference
    public Balloon_Spawner balloonSpawner;


    void Start()
    {
        // Initially hide the resume button
        resumeButton.gameObject.SetActive(false); 

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
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
            PauseGame(); // Pause the game
            GameOver();  // Call the GameOver method
        }
    }

    public void PauseGame()
    {
        // pauseMenuUI.SetActive(true);
        pauseButton.gameObject.SetActive(false); // Hide pause button
        resumeButton.gameObject.SetActive(true); // Show resume button
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        // pauseMenuUI.SetActive(false);
        pauseButton.gameObject.SetActive(true); // Show pause button
        resumeButton.gameObject.SetActive(false); // Hide resume button
        Time.timeScale = 1f;
        isPaused = false;
    }

     public void GameOver()
    {
        // You can add any game over logic here, like displaying a "Game Over" UI

        // Load the game over scene
        SceneManager.LoadScene(gameOverSceneName); 
    }
}