using UnityEngine;

public class Balloon_Manager : MonoBehaviour
{
    public float speed = 2f;
    public float boundaryX = 10f; // X boundary limit
    public int points = 10; // Default points

    private Camera mainCamera;
    private bool movingRight = true; // Flag to track movement direction

    void Start()
    {
        mainCamera = Camera.main;

        // Calculate direction towards the center of the screen
        Vector3 screenCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)); // 10f is an arbitrary distance from the camera
        Vector3 direction = (screenCenter - transform.position).normalized;
        direction.z = 0f; // Ensure movement is only in x and y

        // Set the balloon's initial direction
        transform.right = direction; 
    }

    void Update()
    {
        // Move the balloon
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else 
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Bounce back at the boundary
        if (transform.position.x > boundaryX) 
        {
            transform.position = new Vector3(boundaryX - 0.1f, transform.position.y, transform.position.z);
            movingRight = false; // Change direction immediately
        }
        else if (transform.position.x < -boundaryX) 
        {
            transform.position = new Vector3(-boundaryX + 0.1f, transform.position.y, transform.position.z);
            movingRight = true; // Change direction immediately
        }
    }
}