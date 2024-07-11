using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] GameObject virtualCamera;
    Rigidbody2D rb;
    float movement;
    bool facingRight = true;

    void Start()
    {
        virtualCamera.GetComponent<CameraController>().follow = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
    }
    void Movement() {
        movement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * movement, rb.velocity.y);
        if (movement > 0 && !facingRight || movement < 0 && facingRight) Flip(); 
    }

    void Flip() {
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.GetComponent<Platform>() && rb.velocity.y <= 0) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
