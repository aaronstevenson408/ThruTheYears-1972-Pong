using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public GameManager.Player player;

    public float speed = 500f;
    public bool isLeftPlayer;

    private Rigidbody2D rb;
    private PaddleControls controls;
    private Vector2 moveInput;
    private bool isCollidingWithTopWall = false;
    private bool isCollidingWithBottomWall = false;

    private void Awake()
    {
        controls = new PaddleControls();

        if (isLeftPlayer)
        {
            controls.LeftPlayer.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.LeftPlayer.Move.canceled += ctx => moveInput = Vector2.zero;
        }
        else
        {
            controls.RightPlayer.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.RightPlayer.Move.canceled += ctx => moveInput = Vector2.zero;
        }

        Debug.Log($"Paddle Awake: isLeftPlayer = {isLeftPlayer}");
    }

    private void OnEnable()
    {
        controls.Enable();
        Debug.Log("Paddle controls enabled");
    }

    private void OnDisable()
    {
        controls.Disable();
        Debug.Log("Paddle controls disabled");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        Debug.Log($"Paddle Start: Position = {rb.position}, Collision Mode = {rb.collisionDetectionMode}, Constraints = {rb.constraints}");
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(0, moveInput.y * speed * Time.fixedDeltaTime);

        // Allow movement unless it's trying to move further into a wall
        if ((isCollidingWithTopWall && movement.y > 0) || (isCollidingWithBottomWall && movement.y < 0))
        {
            rb.velocity = Vector2.zero; // Stop movement into the wall
        }
        else
        {
            rb.velocity = movement; // Apply normal movement
        }

        Debug.Log($"Paddle FixedUpdate: MoveInput = {moveInput}, Velocity = {rb.velocity}");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                isCollidingWithTopWall = true; // Paddle is colliding with the top wall
            }
            else
            {
                isCollidingWithBottomWall = true; // Paddle is colliding with the bottom wall
            }

            Debug.Log("Paddle collided with wall.");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                isCollidingWithTopWall = true; // Paddle is still colliding with the top wall
            }
            else
            {
                isCollidingWithBottomWall = true; // Paddle is still colliding with the bottom wall
            }

            Debug.Log("Paddle is still colliding with the wall.");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                isCollidingWithTopWall = false; // Paddle has stopped colliding with the top wall
            }
            else
            {
                isCollidingWithBottomWall = false; // Paddle has stopped colliding with the bottom wall
            }

            Debug.Log("Paddle stopped colliding with wall.");
        }
    }
}
