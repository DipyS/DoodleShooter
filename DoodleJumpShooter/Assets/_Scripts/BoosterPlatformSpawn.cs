using System.Collections;
using UnityEngine;

public class BoosterPlatformSpawn : Boosters
{
    [SerializeField] GameObject platform;
    [SerializeField] AudioClip generateSound;
    [SerializeField] float spawnSpeed = 0.3f;
    [SerializeField] float spawnIntervall = 1.2f;
    [SerializeField] int spawnCount = 5;

    public override void OnActivate()
    {
        base.OnActivate();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        float currentSpawnIntervall = 0;
        for (int i = 0; i < spawnCount; i++) {
            GameManager.Instance.PlaySound(generateSound);
            Vector2 newPlatform1 = new Vector2(transform.position.x + currentSpawnIntervall, transform.position.y);
            Vector2 newPlatform2 = new Vector2(transform.position.x - currentSpawnIntervall, transform.position.y);
            var newPlatform = Instantiate(platform, newPlatform1, Quaternion.identity);
            GameManager.objects.Add(newPlatform);
            newPlatform = Instantiate(platform, newPlatform2, Quaternion.identity);
            GameManager.objects.Add(newPlatform);

            currentSpawnIntervall += spawnIntervall / 2;
            yield return new WaitForSeconds(spawnSpeed);
        }
    }
}
