using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] float speed;
    Transform target;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update() {
        Vector2 direction = new Vector2(transform.position.x-target.position.x, transform.position.y-target.position.y);
        rb.velocity = direction.normalized * speed;
    }
}
