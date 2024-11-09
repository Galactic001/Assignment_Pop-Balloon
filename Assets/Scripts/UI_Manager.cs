using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UI_Manager : MonoBehaviour
{
    // Serialized Button variables
    [SerializeField] public Button playButton;
    [SerializeField] public Button settingsButton;
    [SerializeField] public Button exitButton;

    // Serialized string variable
    [SerializeField] public String scene;


    void Start()
    {
        
    }


    void Update()
    {
        // Listeners for the buttons
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);

    }

    // Function to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(scene);  
    }

    public void OpenSettings()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
