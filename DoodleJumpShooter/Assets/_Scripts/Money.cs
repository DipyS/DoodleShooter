using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class Money : MonoBehaviour
{
    [SerializeField, Range(1, 100)] int moneyAmount = 1;
    [SerializeField] float flyForce = 7;
    [SerializeField] float followSpeed = 3;
    [SerializeField] float followStartDelay = 1.5f;
    [SerializeField] ParticleSystem collectParticles;
    float timer;
    Rigidbody2D rb;
    void Start()
    {
        timer = followStartDelay;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Random.Range(-flyForce, flyForce), Random.Range(-1,3));
        rb.AddTorque(Random.Range(-1000, 1000));
    }

    void Update()
    {
        if (timer <= 0) {
            Vector2 floatDirection = new Vector2(transform.position.x - GameManager.Instance.player.transform.position.x, transform.position.y - GameManager.Instance.player.transform.position.y);
            rb.velocity = floatDirection.normalized * followSpeed;
        }
        else {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()) {
            if (collectParticles != null) Instantiate(collectParticles, transform.position, Quaternion.identity);
            Inventory.singleton.Money += moneyAmount;
            Destroy(gameObject);
        }    
    }
}
