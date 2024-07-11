using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] ParticleSystem damageParticles;
    [SerializeField] ParticleSystem killParticles;
    [SerializeField] GameObject floatingText;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int armor;
    public virtual void TakeDamage(int damage) {
        Instantiate(damageParticles, transform.position,Quaternion.identity);
        damage = damage - (damage / 100 * armor); //Применение поглощения урона:000
        if (damage <= 0) damage = 1;

        Instantiate(floatingText, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = damage.ToString();

        health -= damage;
        if (health <= 0) Kill(); //Смэрт
    }
    public virtual void Heal(int points) {
        if (points <= 0) points = 1;
        health += points;
    }

    public virtual void Kill() {
        Instantiate(killParticles, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
