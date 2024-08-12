using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    public bool isLeftPlayer;

    private Rigidbody2D rb;
    private PaddleControls controls;
    private Vector2 moveInput;
    private bool isCollidingWithWall = false;

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
        if (!isCollidingWithWall)
        {
            Vector2 movement = new Vector2(0, moveInput.y * speed * Time.fixedDeltaTime);
            rb.velocity = new Vector2(0, movement.y); // Update velocity directly
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement if colliding with wall
        }

        Debug.Log($"Paddle FixedUpdate: MoveInput = {moveInput}, Velocity = {rb.velocity}");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Paddle OnCollisionEnter2D: Collided with {collision.gameObject.name}, Tag = {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
            // Optionally, set vertical velocity to zero or adjust as needed
            rb.velocity = new Vector2(rb.velocity.x, 0);
            Debug.Log("Paddle collided with wall. Stopping movement.");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"Paddle OnCollisionStay2D: Still colliding with {collision.gameObject.name}, Tag = {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"Paddle OnCollisionExit2D: Stopped colliding with {collision.gameObject.name}, Tag = {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = false;
        }
    }
}
