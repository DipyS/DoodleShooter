using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    EnemiesHealthBar spawnHealthBar;
    [SerializeField] protected bool contanctDamage = true;
    [SerializeField] protected int damage = 1;
    protected Slider healthBar;

    public new void Start()
    {
        base.Start();
        spawnHealthBar = Resources.Load<EnemiesHealthBar>("Prefabs/HealthBar");
        if (spawnHealthBar != null) Instantiate(spawnHealthBar, transform.position, Quaternion.identity).enemy = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (contanctDamage && other.gameObject.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().Undieing) {
            GameManager.Instance.player.TakeDamage(damage);
        }
    }
}
