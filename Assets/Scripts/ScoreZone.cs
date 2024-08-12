using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public GameManager.Player scoringPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if (scoringPlayer != null)
            {
                GameManager.Instance.AddScore(scoringPlayer);
            }
            else
            {
                Debug.LogError("Scoring player not set for this ScoreZone.");
            }

            Ball ball = other.GetComponent<Ball>();
            if (ball != null)
            {
                ball.LaunchBall();
            }
            else
            {
                Debug.LogError("Ball component not found on the colliding object.");
            }
        }
    }
}