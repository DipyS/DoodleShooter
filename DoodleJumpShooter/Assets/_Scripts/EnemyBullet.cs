using UnityEngine;

public class EnemyBullet : Entity
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] ParticleSystem particle;
    void Start()
    {
        GameManager.objects.Add(gameObject);
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Invoke(nameof(DestroyBullet),lifeTime);
    }

    void DestroyBullet() {
        if (particle != null) Instantiate(particle,transform.position,transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().Undieing) {
            DestroyBullet();
            GameManager.Instance.Lose();
        }
    }

    public override void TakeDamage(int damage)
    {
        DestroyBullet();
        base.TakeDamage(damage);
    }
}
