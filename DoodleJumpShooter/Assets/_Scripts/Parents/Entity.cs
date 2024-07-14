using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] List<Rigidbody2D> slisedSides;
    [SerializeField] float MaxForcePart = 4;
    [SerializeField] float MaxRotationForcePart = 10;
    [SerializeField,Space(5)] ParticleSystem damageParticles;
    [SerializeField] ParticleSystem killParticles;
    [SerializeField] GameObject floatingText;
    [SerializeField,Space(5)] protected int health = 1;
    [SerializeField] protected int armor;
    public virtual void TakeDamage(int damage) {
        Instantiate(damageParticles, transform.position,Quaternion.identity);
        var newDamage = damage - ((float)damage / 100 * armor); //Применение поглощения урона:000
        if (newDamage <= 0) newDamage = 1;

        Instantiate(floatingText, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = Mathf.Round(newDamage).ToString();

        health -= (int)newDamage;
        if (health <= 0) Kill(); //Смэрт
    }
    public virtual void Heal(int points) {
        if (points <= 0) points = 1;
        health += points;
    }

    public virtual void Kill() {
        foreach (var p in slisedSides)
        {
            Rigidbody2D newPart = Instantiate(p, transform.position,Quaternion.identity);
            newPart.velocity = new Vector2(Random.Range(-MaxForcePart,MaxForcePart),MaxForcePart);
            newPart.rotation = Random.Range(-MaxRotationForcePart,MaxRotationForcePart);
            Destroy(newPart.gameObject,4);
        }
        CameraShake.singleton.Shake(0.3f,4);

        Instantiate(killParticles, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
