using UnityEngine;

public class Explotion : MonoBehaviour
{
    [SerializeField] float explotionRadius;
    [SerializeField] int damage = 450;
    void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explotionRadius);
        foreach (var item in colliders)
        {
            if (item.gameObject.TryGetComponent(out Entity entity)) {
                entity.TakeDamage(damage + Random.Range(-5,5));
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explotionRadius);
    }
}
