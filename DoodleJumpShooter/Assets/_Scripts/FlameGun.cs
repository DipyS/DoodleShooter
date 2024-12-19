using UnityEngine;

public class FlameGun : Weapon
{
    ParticleSystem flame;
    [SerializeField] int damage = 45;
    [SerializeField] float damageRadius = 3;
    [SerializeField] float damageRadius2 = 3;
    [SerializeField] Transform checkPos;
    [SerializeField] Transform checkPos2;

    void Awake()
    {
        flame = GetComponentInChildren<ParticleSystem>();
        flame.Pause();
        flame.GetComponentInChildren<ParticleSystem>().Pause();
    }

    protected override void Shoot()
    {
        flame.Play();
        flame.GetComponentInChildren<ParticleSystem>().Play();

        Collider2D[] collision = Physics2D.OverlapCircleAll(checkPos.position, damageRadius);
        foreach (Collider2D item in collision) {
            if (item.TryGetComponent(out Entity entity)) {
                entity.TakeDamage(damage);
            }
        }
        
        collision = Physics2D.OverlapCircleAll(checkPos2.position, damageRadius2);
        foreach (Collider2D item in collision) {
            if (item.TryGetComponent(out Entity entity)) {
                entity.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkPos.position, damageRadius);
        Gizmos.DrawWireSphere(checkPos2.position, damageRadius2);
    }

}
