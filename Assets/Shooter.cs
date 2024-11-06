using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Camera mainCamera;
    public float raycastDistance = 100f;
    public LineRenderer lineRenderer;

    private bool isShooting;

    // Audio
    public AudioSource shooterAudioSource;
    public AudioClip shootSound;
    public AudioClip balloonPopSound; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            ShootRaycast();
        }

        lineRenderer.enabled = isShooting;

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }

    void ShootRaycast()
    {
        // Use ScreenToWorldPoint for 2D
        Vector3 touchPosition = Input.mousePosition;
        touchPosition.z = 10f; // Distance from the camera
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        // Create a 2D ray
        Ray2D ray = new Ray2D(worldPosition, Vector2.zero); // Direction is not important in 2D

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance);

         // Play shoot sound
        shooterAudioSource.PlayOneShot(shootSound); 

        if (hit.collider != null && hit.collider.CompareTag("Balloon"))
        {
             // Get the points from the balloon
            int pointsToAdd = hit.collider.gameObject.GetComponent<Balloon_Manager>().points; 

            // Add the points to the score
            Score_Manager.Instance.AddScore(pointsToAdd);

            hit.collider.gameObject.SetActive(false);
            // Destroy(hit.collider.gameObject);


             // Play balloon pop sound
            AudioSource.PlayClipAtPoint(balloonPopSound, hit.point);

            // Show the raycast line in 2D
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

             // Rotate the sprite towards the touch position
            Vector2 direction = hit.point - (Vector2)transform.position; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);   

        }
        else
        {
            // If no hit, draw the line to max distance
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, worldPosition);


             // Rotate towards the touch position even if no balloon is hit
            Vector2 direction = worldPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);   

        }
    }
}