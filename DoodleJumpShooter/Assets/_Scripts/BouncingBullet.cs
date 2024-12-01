using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Bullet
{
    [SerializeField] int bouncingCount = 2;
    List<Enemy> hittedEnemies = new List<Enemy>();
    protected override void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.TryGetComponent(out Entity hit))
        {
            foreach (Entity enemy in hittedEnemies)
            {
                if (enemy == null || enemy == hit) return;  
            }
            hit.TakeDamage(damage);
            if (hitParticles != null) Instantiate(hitParticles,transform.position, Quaternion.identity);
            BounceToNextEnemy(hit);
            if (bouncingCount <= 0) DestroyBullet();
        }
    }

    void BounceToNextEnemy(Entity hit) {
        List<Entity> enemies = new List<Entity>();

        foreach (GameObject obj in GameManager.objects)
        {
            if (obj == null) continue;
            if (obj.TryGetComponent(out Entity enemy) && enemy.gameObject != hit.gameObject) {
                enemies.Add(enemy);
            }
        }

        Entity nearestEnemy = new Entity();
        float nearestDistance = float.MaxValue;

        foreach (Entity enemy in enemies)
        {
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy == null) return;
        Vector2  differnce = transform.position - nearestEnemy.transform.position;
        float angle = Mathf.Atan2(differnce.y, differnce.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle + 180);
        rb.velocity = transform.right * speed;

        bouncingCount--;
    }
}
