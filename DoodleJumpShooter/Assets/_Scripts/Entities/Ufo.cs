using System.Collections;
using UnityEngine;

public class Ufo : Enemy
{
    [SerializeField] UfoChelik chelik;
    [SerializeField] EnemyBullet shotPrefab;
    [SerializeField] float floatingSpeed = 1f;
    [SerializeField] float DistanceToShot = 1f;
    [SerializeField] float shotOffsetAngle = 10f;
    [SerializeField] float shotIntervall = 0.5f;
    [SerializeField] AudioClip teleportSound;
    [SerializeField] GameObject pumpingLight;

    float shotTimer;
    Animator anim;
    Rigidbody2D rb;
    Coroutine teleportProcces;

    public new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (chelik == null || !chelik.isLerping) 
        {
            //Основная Логика
            if (chelik != null) Destroy(chelik.gameObject);
            if (pumpingLight != null) Destroy(pumpingLight);
            if (GameManager.Instance.player.transform.position.y > transform.position.y) 
            {
                RandomTeleport();
            }

            if (GameManager.Instance.player.transform.position.x > transform.position.x) 
            {
                rb.velocity = new Vector2(floatingSpeed, rb.velocity.y);
            }
            else 
            {
                rb.velocity = new Vector2(-floatingSpeed, rb.velocity.y);
            }

            float differenceX = GameManager.Instance.player.transform.position.x - transform.position.x;
            if (differenceX < 0) differenceX *= -1;
            if (differenceX <= DistanceToShot) 
            {
                if (shotTimer <= 0) 
                {
                    shotTimer = shotIntervall;
                    Shot();
                }
                else shotTimer -= Time.deltaTime;
            }
        }
    }

    void Shot() 
    {
        var newBullet = Instantiate(shotPrefab, transform.position, Quaternion.identity);
        newBullet.transform.rotation = Quaternion.Euler(0, 0, 90 + Random.Range(-shotOffsetAngle / 2, shotOffsetAngle / 2));
    }

    void RandomTeleport() 
    {
        if (teleportProcces != null) return;
        teleportProcces = StartCoroutine(IRandomTeleport());
    }

    IEnumerator IRandomTeleport() 
    {
        anim.SetTrigger("TeleportHide");
        Blink();
        GameManager.Instance.PlaySound(teleportSound);
        yield return new WaitForSeconds(1);
        
        transform.position = new Vector2(Random.Range(-GameManager.Instance.widthSpawn, GameManager.Instance.widthSpawn), Camera.main.transform.position.y + GameManager.Instance.height);
        
        anim.SetTrigger("TeleportShow");
        Blink();
        teleportProcces = null;
    }
}
