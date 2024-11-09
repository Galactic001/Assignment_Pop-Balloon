using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Camera mainCamera;
    public float raycastDistance = 100f;
    public LineRenderer lineRenderer;

    private bool isShooting;
    private int consecutiveHits = 0; // To track consecutive hits

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
        Vector3 touchPosition = Input.mousePosition;
        touchPosition.z = 10f; 
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        Ray2D ray = new Ray2D(worldPosition, Vector2.zero); 

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance);

        shooterAudioSource.PlayOneShot(shootSound); 

        if (hit.collider != null && hit.collider.CompareTag("Balloon"))
        {
            int pointsToAdd = hit.collider.gameObject.GetComponent<Balloon_Manager>().points; 

            // Pass true for consecutive hit
            Score_Manager.Instance.AddScore(pointsToAdd, true); 
            consecutiveHits++;

            hit.collider.gameObject.SetActive(false);
            Instantiate(hit.collider.gameObject.GetComponent<Balloon_Manager>().balloonpop, hit.point, Quaternion.identity);

            AudioSource.PlayClipAtPoint(balloonPopSound, hit.point);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            Vector2 direction = hit.point - (Vector2)transform.position; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  
        }
        else
        {
            // Pass false to indicate the streak is broken
            Score_Manager.Instance.AddScore(0, false); 
            consecutiveHits = 0; // Reset consecutive hits

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, worldPosition);

            Vector2 direction = worldPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        }
    }
}