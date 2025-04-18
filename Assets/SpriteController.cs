using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public float Speed = 3f;
    public float JumpForce = 5f;

    private Vector3 moveDirection;
    private GameManager gameManager;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector3 direction, GameManager manager)
    {
        moveDirection = direction;
        gameManager = manager;
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    void Update()
    {
        // Move sprite
        transform.position += moveDirection * Speed * Time.deltaTime;

        // Jump logic
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if ((clickPosition.x < 0 && moveDirection == Vector3.right) || 
                (clickPosition.x > 0 && moveDirection == Vector3.left))
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        rb.velocity = Vector2.up * JumpForce;
        gameManager.JumpSFX();
    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // Check for collision with other sprite
    //     if (collision.CompareTag("Sprite"))
    //     {
    //         gameManager.GameOver();
    //     }
    // }
   private void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.tag == "Sprite")
    {
        gameManager.GameOver();
    }
   }

    void OnBecameInvisible()
    {
        // When sprite exits screen, award point
        if (gameManager != null)
            gameManager.AddScore();
    }
}
