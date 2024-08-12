using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedIncrease = 0.5f;
    public int maxConsecutiveWallHits = 10;

    [SerializeField] private float currentSpeed;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private int consecutiveWallHits = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = initialSpeed;
        LaunchBall();
    }

    public void LaunchBall()  // Changed to public
    {
        transform.position = Vector2.zero;
        consecutiveWallHits = 0;
        currentSpeed = initialSpeed;

        Vector2 direction;
        do
        {
            float x = Random.Range(0, 2) == 0 ? -1 : 1;
            float y = Random.Range(-1f, 1f);
            direction = new Vector2(x, y).normalized;
        } while (Mathf.Abs(direction.x) < 0.2f || Mathf.Abs(direction.y) < 0.2f);

        rb.velocity = direction * currentSpeed;
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            HandlePaddleCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
    }

    void HandlePaddleCollision(Collision2D collision)
    {
        consecutiveWallHits = 0;

        currentSpeed = Mathf.Min(currentSpeed + speedIncrease, maxSpeed);

        float y = HitFactor(transform.position,
                            collision.transform.position,
                            collision.collider.bounds.size.y);

        float paddleX = collision.transform.position.x;
        float screenCenterX = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)).x;

        float dirX = (paddleX < screenCenterX) ? 1 : -1;

        Vector2 dir = new Vector2(dirX, y).normalized;
        rb.velocity = dir * currentSpeed;

        Debug.Log($"Paddle hit. New speed: {currentSpeed}");
    }

    void HandleWallCollision(Collision2D collision)
    {
        consecutiveWallHits++;
        if (consecutiveWallHits >= maxConsecutiveWallHits)
        {
            LaunchBall();
            return;
        }

        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        direction += new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        direction.Normalize();

        rb.velocity = direction * currentSpeed;
    }

    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }
}