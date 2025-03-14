using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public ParticleSystem blood;
    [SerializeField] protected List<Rigidbody2D> slisedSides;

    [SerializeField] protected float MaxForcePart = 4;
    [SerializeField] protected float MaxRotationForcePart = 10;

    [SerializeField,Space(5)] protected ParticleSystem damageParticles;
    [SerializeField] protected ParticleSystem killParticles;

    [Space(5)] public int health = 1;
    [SerializeField] protected int armor;
    [SerializeField,Space(10)] protected int droppingMoneyCount;

    protected Money money;
    protected SpriteRenderer spriteRenderer;
    protected Material defauldMaterial;
    protected Material blink;
    protected GameObject floatingText;
    protected GameObject floatingCrit;
    [SerializeField] AudioClip deathSound;

    public void Start()
    {
        floatingText = Resources.Load<GameObject>("Prefabs/floatingText");
        floatingCrit = Resources.Load<GameObject>("Prefabs/floatingCrit");
        defauldMaterial = Resources.Load<Material>("Materials/DipyDefauld");
        blink = Resources.Load<Material>("Materials/Blink");
        money = Resources.Load<Money>("Prefabs/Money");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
    }
    public void Awake()
    {
        floatingText = Resources.Load<GameObject>("Prefabs/floatingText");
        defauldMaterial = Resources.Load<Material>("Materials/DipyDefauld");
        blink = Resources.Load<Material>("Materials/Blink");
        money = Resources.Load<Money>("Prefabs/Money");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
    }

    public virtual void TakeDamage(int damage) {
        if (health <= 0) return;

        StartCoroutine(Blink());
        if (damageParticles != null) Instantiate(damageParticles, transform.position, Quaternion.identity);
        var newDamage = damage - ((float)damage / 100 * armor); //Применение поглощения урона:000
        if (newDamage <= 0) newDamage = 1;
        if (Random.Range(1,11) <= 2) {
            newDamage *= 2;
            if (floatingCrit != null) Instantiate(floatingCrit, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = Mathf.Round(newDamage).ToString() + "!";
        } else {
            if (floatingText != null) Instantiate(floatingText, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = Mathf.Round(newDamage).ToString();
        }
        
        health -= (int)newDamage;
        if (health <= 0) Kill(); //Смэрт
    }
    public virtual void Heal(int points) {
        if (points <= 0) points = 1;
        health += points;
    }

    public virtual void Kill() {
        GameManager.Instance.PlaySound(deathSound);
        DropMoney();
        DropParts();
        CameraShake.singleton.Shake(0.3f,4);

        if (killParticles != null) Instantiate(killParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected IEnumerator Blink() {
        if (spriteRenderer != null && defauldMaterial != null && blink != null) {
            spriteRenderer.material = blink;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material = defauldMaterial;
        }
    }
    
    protected void DropMoney() {
        for (int i = 0; i < droppingMoneyCount; i++) {
            Instantiate(money, transform.position, Quaternion.identity);
        }
    }
    
    protected void DropParts() {
        foreach (var p in slisedSides)
        {
            Rigidbody2D newPart = Instantiate(p, transform.position,Quaternion.identity);
            
            Vector3 difference = transform.position - GameManager.Instance.player.transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            newPart.transform.rotation = Quaternion.Euler(0,0,angle + Random.Range(-20f,21f));

            newPart.velocity = newPart.transform.right * Random.Range(MaxForcePart, MaxForcePart + 10f);
            newPart.AddTorque(Random.Range(-MaxRotationForcePart,MaxRotationForcePart));
            GameManager.objects.Add(newPart.gameObject);

            if (blood != null) Instantiate(blood, newPart.transform);
            Destroy(newPart.gameObject,4);
        }
    }
}
