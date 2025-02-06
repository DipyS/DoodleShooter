using System.Collections.Generic;
using UnityEngine;
public class ExlosiveBullet : Bullet
{
    [SerializeField] int spawnCount;
    [SerializeField] Bullet spawnPrefab;
    List<Entity> allEnemyies = new List<Entity>();


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Entity>() != null) ExlposiveToParts(other.gameObject);
        base.OnTriggerEnter2D(other);
    }

    void ExlposiveToParts(GameObject other) {
        foreach (var item in GameManager.objects)
        {
            if (item != null && item.TryGetComponent(out Entity enemy)) {
                allEnemyies.Add(enemy);
            }
        }

        if (allEnemyies.Count < 1) return;

        for (int currentBullet = 0; currentBullet < spawnCount; currentBullet++)
        {
            int randIndex = Random.Range(0, allEnemyies.Count);
            Bullet newBullet = Instantiate(spawnPrefab, transform.position, Quaternion.identity);
            newBullet.ignoreObject = other;

            Vector2 different = allEnemyies[randIndex].transform.position - transform.position;
            float angle = Mathf.Atan2(different.y, different.x) * Mathf.Rad2Deg;

            newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
