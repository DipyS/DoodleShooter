using UnityEngine;
using UnityEngine.UI;

public class EnemiesHealthBar : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] Vector2 offset = new Vector2(0.5f, 1.5f);
    [SerializeField] Slider healthBar;

    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = enemy.health;
        healthBar.value = enemy.health;
    }

    void Update()
    {
        if (enemy != null) {
            transform.position = new Vector2(enemy.transform.position.x +  offset.x, enemy.transform.position.y + offset.y);
            healthBar.value = enemy.health;
        } else {
            Destroy(gameObject);
        }
    }
}
