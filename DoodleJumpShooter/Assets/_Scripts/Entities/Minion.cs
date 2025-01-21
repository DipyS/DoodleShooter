using UnityEngine;

public class Minion : Enemy
{
    [SerializeField] GameObject booster;
    [SerializeField] float dashForce = 10;
    [SerializeField] float timeToAttack = 5;
    float timer;
    Rigidbody2D rb;

    void Start()
    {
        GameManager.objects.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        timer = timeToAttack + Random.Range(1,3);
        rb.velocity = new Vector2(Random.Range(-7,8), -1);
    }

    void Update()
    {
        if (timer <= 0) {
            rb.velocity = transform.right * dashForce;
            timer = timeToAttack + Random.Range(1,3);
        } else {
            LookToPlayer();
            timer -= Time.deltaTime;
        }
    }

    void LookToPlayer() {
        Vector2 differencePos = new Vector2(transform.position.x - GameManager.Instance.player.transform.position.x, transform.position.y - GameManager.Instance.player.transform.position.y);
        float angle = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle + 180);
    }

    public override void Kill()
    {
        if (booster != null) GameManager.objects.Add(Instantiate(booster, transform.position, Quaternion.identity));
        base.Kill();
    }
}
