using UnityEngine;

public class Granade : Bullet
{
    [SerializeField] Explotion explotion;

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Enemy>()) {
            DestroyBullet();
        }
    }
    protected override void DestroyBullet()
    {
        Instantiate(explotion, transform.position, Quaternion.identity);
        base.DestroyBullet();
    }
}
