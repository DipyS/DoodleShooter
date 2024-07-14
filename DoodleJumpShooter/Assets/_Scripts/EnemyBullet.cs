using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] ParticleSystem particle;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Invoke(nameof(DestroyBullet),lifeTime);
    }

    void DestroyBullet() {
        if (particle != null) Instantiate(particle,transform.position,transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()) {
            DestroyBullet();
            GameManager.Instance.Lose();
        }
    }    
}
