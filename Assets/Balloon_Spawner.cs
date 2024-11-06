using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Balloon_Spawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Slider spawnRateSlider;
    public TMP_Text timerText;

    public int minBalloons = 10;
    public int maxBalloons = 50;
    public float spawnInterval = 1f;
    public float maxSpawnTime = 60f; // 1 minute

    private List<GameObject> balloonPool = new List<GameObject>();
    private float screenHeight;
    private float screenWidth;
    public float currentSpawnTime;

    // Scripts Refrence
     public Balloon_Factory balloonFactory;


    private void Awake()
    {
        // Calculate screen dimensions
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        spawnRateSlider.onValueChanged.AddListener(OnSpawnRateChanged);
        OnSpawnRateChanged(spawnRateSlider.value);

        currentSpawnTime = maxSpawnTime;
    }

    private void OnSpawnRateChanged(float value)
    {
        int newCount = Mathf.RoundToInt(Mathf.Lerp(minBalloons, maxBalloons, value));
        AdjustBalloonCount(newCount);
    }

    private void AdjustBalloonCount(int newCount)
    {
        if (newCount > balloonPool.Count)
        {
            for (int i = balloonPool.Count; i < newCount; i++)
            {
                GameObject balloon = Instantiate(balloonPrefab);
                balloon.SetActive(false);
                balloonPool.Add(balloon);
            }
        }
        else if (newCount < balloonPool.Count)
        {
            // Destroy and remove excess balloons from the pool
            for (int i = balloonPool.Count - 1; i >= newCount; i--) 
            {
                if (balloonPool[i].activeInHierarchy)
                {
                    Destroy(balloonPool[i]);
                }
                balloonPool.RemoveAt(i); 
            }
        }
    }

    private void Update()
    {
        // Timer
        currentSpawnTime -= Time.deltaTime;
        if (currentSpawnTime <= 0f)
        {
            currentSpawnTime = 0f; // Prevent negative time
            // You might want to add a game over condition here
        }

        // Update timer text
        timerText.text = "Time: " + Mathf.RoundToInt(currentSpawnTime).ToString();

        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0f)
        {
            SpawnBalloon();
            spawnInterval = Mathf.Lerp(0.1f, 2f, spawnRateSlider.value);
        }
    }

    private void SpawnBalloon()
    {
        GameObject balloon = GetFactoryBalloon(); 
        // GameObject balloon = GetPooledBalloon();
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
        }
    }

    private GameObject GetFactoryBalloon()
    {
        for (int i = 0; i < balloonPool.Count; i++)
        {
            if (!balloonPool[i].activeInHierarchy)
            {
                // Get a random balloon type
                BalloonType randomType = (BalloonType)Random.Range(0, System.Enum.GetValues(typeof(BalloonType)).Length);
                balloonPool[i] = balloonFactory.CreateBalloon(randomType);
                return balloonPool[i];
            }
        }
        return null;
    }

    private GameObject GetPooledBalloon()
    {
        for (int i = 0; i < balloonPool.Count; i++)
        {
            if (!balloonPool[i].activeInHierarchy)
                return balloonPool[i];
        }
        return null;
    }
}