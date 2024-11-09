using UnityEngine;

public class Balloon_Manager : MonoBehaviour
{
    public float speed = 2f;
    public int points = 10;
    public int timePenalty = 5; // Time penalty for this balloon type

    public GameObject cannon; // Reference to the cannon GameObject
    public float waveFrequency = 5f; // Frequency of the wave motion
    public float waveAmplitude = 1f; // Amplitude of the wave motion
    public float zigzagFrequency = 4f; // Frequency of the zigzag motion
    public float zigzagAmplitude = 2f; // Amplitude of the zigzag motion

    private Vector3 direction;
    private float startTime;
    private int movementType; // 0 = Straight, 1 = Wavy, 2 = Zigzag

    private Balloon_Spawner spawner; // Reference to the spawner
    public GameObject balloonpop;

    void Start()
    {
        // Find the cannon GameObject with the tag "Cannon"
        cannon = GameObject.FindWithTag("Cannon");
        spawner = FindFirstObjectByType<Balloon_Spawner>(); // Get the spawner reference

        if (cannon == null)
        {
            Debug.LogError("Cannon not found! Make sure it has the tag 'Cannon'.");
            return;
        }

        // Calculate initial direction towards the cannon
        direction = (cannon.transform.position - transform.position).normalized;

        startTime = Time.time;

        // Randomly select a movement type
        movementType = Random.Range(0, 3);
    }

    void Update()
    {
        // Move the balloon based on the selected movement type
        switch (movementType)
        {
            case 0: // Straight
                transform.Translate(direction * speed * Time.deltaTime);
                break;
            case 1: // Wavy
                transform.Translate(direction * speed * Time.deltaTime);
                transform.position += Vector3.up * Mathf.Sin((Time.time - startTime) * waveFrequency) * waveAmplitude * Time.deltaTime;
                break;
            case 2: // Zigzag
                transform.Translate(direction * speed * Time.deltaTime);
                transform.position += Vector3.right * Mathf.Sin((Time.time - startTime) * zigzagFrequency) * zigzagAmplitude * Time.deltaTime;
                break;
        }

         // Check for collision with the cannon
        if (cannon != null && Vector3.Distance(transform.position, cannon.transform.position) < 1f) 
        {
            BurstBalloon();
        }
    }


    private void BurstBalloon()
    {
        // Reduce time in the spawner
        if (spawner != null && balloonpop != null)
        {
            spawner.currentSpawnTime -= timePenalty;
            Instantiate(balloonpop, transform.position, Quaternion.identity);
        }

        // Return the balloon to the pool
        if (spawner != null)
        {
            spawner.ReturnBalloonToPool(gameObject);
        }
    }
}