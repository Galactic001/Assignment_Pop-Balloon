using UnityEngine;


public enum BalloonType
{
    Red,
    Blue,
    Green
}

// Abstract Balloon Factory
public abstract class AbsBalloonFactory : MonoBehaviour
{
    public abstract GameObject CreateBalloon(BalloonType type);
}

// Concrete Balloon Factory
public class Balloon_Factory : AbsBalloonFactory
{
    // Different prefabs for each balloon type
    public GameObject redBalloonPrefab;
    public GameObject blueBalloonPrefab;
    public GameObject greenBalloonPrefab;

    public override GameObject CreateBalloon(BalloonType type)
    { 
        GameObject balloon = null;

        switch (type)
        {
            case BalloonType.Red:
                balloon = Instantiate(redBalloonPrefab);
                balloon.GetComponent<Balloon_Manager>().speed = 2f;
                balloon.GetComponent<Balloon_Manager>().points = 10;
                break;
            case BalloonType.Blue:
                balloon = Instantiate(blueBalloonPrefab);
                balloon.GetComponent<Balloon_Manager>().speed = 4f;
                balloon.GetComponent<Balloon_Manager>().points = 20;
                break;
            case BalloonType.Green:
                balloon = Instantiate(greenBalloonPrefab);
                balloon.GetComponent<Balloon_Manager>().speed = 6f;
                balloon.GetComponent<Balloon_Manager>().points = 30;
                break;
        }

        return balloon;
    }
}