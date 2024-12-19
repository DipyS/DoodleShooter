using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected GameObject hitParticles;
    [SerializeField] private float lifeTime = 5;
    [SerializeField] AudioClip hitSound;
    protected Rigidbody2D rb;
    

    protected virtual void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Invoke(nameof(DestroyBullet),lifeTime + Random.Range(0,0.3f));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Entity hit))
        {
            GameManager.Instance.PlaySound(hitSound);
            hit.TakeDamage(damage);
            DestroyBullet();
        }
    }

    protected virtual void DestroyBullet() {
        if (hitParticles != null) Instantiate(hitParticles,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
