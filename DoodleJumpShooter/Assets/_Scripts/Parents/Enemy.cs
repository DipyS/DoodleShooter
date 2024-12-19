using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    [SerializeField] EnemiesHealthBar spawnHealthBar;
    [SerializeField] protected bool contanctDamage = true;
    protected Slider healthBar;

    void Start()
    {
        if (spawnHealthBar != null) Instantiate(spawnHealthBar, transform.position, Quaternion.identity).enemy = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (contanctDamage && other.gameObject.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().Undieing) {
            GameManager.Instance.Lose();
        }
    }
}
