using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 15f;
    public float speedIncrease = 0.1f;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    void LaunchBall()
    {
        transform.position = Vector2.zero;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(x, y).normalized;
        rb.velocity = direction * speed;
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        speed = Mathf.Min(speed + speedIncrease, maxSpeed);

        if (collision.gameObject.CompareTag("Paddle"))
        {
            float y = HitFactor(transform.position,
                                collision.transform.position,
                                collision.collider.bounds.size.y);

            Vector2 dir = new Vector2(rb.velocity.x > 0 ? 1 : -1, y).normalized;
            rb.velocity = dir * speed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rb.velocity = direction * speed;
        }
    }

    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }
}