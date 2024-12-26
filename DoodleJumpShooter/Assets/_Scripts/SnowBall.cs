using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : Bullet
{
    [SerializeField] int superBallChance = 5;
    [SerializeField] int spawnCount = 3;
    [SerializeField] GameObject spawnObject;
    [SerializeField] Sprite superBallSprite;
    [SerializeField] List<Color> randomColors;
    bool isSuberBall;
    SpriteRenderer spriteRenderer;

    new void Start()
    {
        base.Start();

        GameManager.objects.Add(this.gameObject);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1000, 1001));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = randomColors[Random.Range(0, randomColors.Count)];

        if (superBallChance >= Random.Range(0, 101)) {
            isSuberBall = true;
            spriteRenderer.sprite = superBallSprite;
        }
    }

    protected override void DestroyBullet()
    {
        if (isSuberBall) {
            for (int i = 0; i < spawnCount; i++) {
                var snowBall = Instantiate(spawnObject, transform.position, Quaternion.identity);
                snowBall.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 361));
            }
        }
        base.DestroyBullet();
    }
}
