using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Balloon_Spawner : MonoBehaviour
{
    public Slider spawnRateSlider;
    public TMP_Text timerText;

    public int totalBalloons = 100; // Fixed number of balloons to spawn
    public float minSpawnInterval = 0.5f; 
    public float maxSpawnInterval = 4f; 
    public float currentSpawnInterval;
    public float maxSpawnTime = 300f; //  Game Timer
    public float currentSpawnTime;

    private Queue<GameObject> balloonPool = new Queue<GameObject>();
    private float screenHeight;
    private float screenWidth;
    private int balloonsSpawned = 0; 

    public int lvl1 = 200;
    public int lvl2 = 400;

    // Scripts Reference
    public Balloon_Factory balloonFactory;

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        spawnRateSlider.onValueChanged.AddListener(OnSpawnRateChanged);
        OnSpawnRateChanged(spawnRateSlider.value);

        currentSpawnTime = maxSpawnTime;
    }

    private void OnSpawnRateChanged(float value)
    {
        // Calculate spawn interval based on slider value
        currentSpawnInterval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, value);
    }

    private void Update()
    {
        currentSpawnTime -= Time.deltaTime;
        if (currentSpawnTime <= 0f)
        {
            currentSpawnTime = 0f;
            // Game over condition here 
        }

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(currentSpawnTime / 60f);
        int seconds = Mathf.FloorToInt(currentSpawnTime % 60f);

        // Update timer text in "min:sec" format
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds); 

        if (balloonsSpawned < totalBalloons && currentSpawnTime > 0f) 
        {
            currentSpawnInterval -= Time.deltaTime;
            if (currentSpawnInterval <= 0f)
            {
                SpawnBalloon();
                balloonsSpawned++;
                currentSpawnInterval = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, spawnRateSlider.value);
            }
        }
    }

    private void SpawnBalloon()
    {
        GameObject balloon = GetBalloonFromPool();
        if (balloon != null)
        {
            float randomX = Random.Range(-screenWidth, screenWidth);
            float randomY = Random.Range(-screenHeight, screenHeight);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            if (Mathf.Abs(spawnPosition.x) < screenWidth / 2f)
                spawnPosition.x = Mathf.Sign(spawnPosition.x) * (screenWidth / 2f + 1f);

            if (Mathf.Abs(spawnPosition.y) < screenHeight / 2f)
                spawnPosition.y = Mathf.Sign(spawnPosition.y) * (screenHeight / 2f + 1f);

            balloon.transform.position = spawnPosition;
            balloon.SetActive(true);

            Balloon_Manager balloonManager = balloon.GetComponent<Balloon_Manager>();
            if (balloonManager != null)
            {
                balloonManager.cannon = GameObject.FindWithTag("Cannon");
            }
        }
    }

    private GameObject GetBalloonFromPool()
    {
        if (balloonPool.Count > 0)
        {
            GameObject balloon = balloonPool.Dequeue();
            return balloon;
        }
        else
        {
            List<BalloonType> allowedTypes = new List<BalloonType>();
            allowedTypes.Add(BalloonType.Red);

            if (Score_Manager.Instance.GetCurrentScore() > lvl1)
            {
                allowedTypes.Add(BalloonType.Blue);
            }

            if (Score_Manager.Instance.GetCurrentScore() > lvl2)
            {
                allowedTypes.Add(BalloonType.Green);
            }

            BalloonType randomType = allowedTypes[Random.Range(0, allowedTypes.Count)];
            GameObject balloon = balloonFactory.CreateBalloon(randomType);
            return balloon;
        }
    }

    public void ReturnBalloonToPool(GameObject balloon)
    {
        balloon.SetActive(false);
        balloonPool.Enqueue(balloon);
    }
}