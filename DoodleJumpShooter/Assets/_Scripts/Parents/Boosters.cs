using UnityEngine;

public class Boosters : Entity
{
    [SerializeField] AudioClip activateSound;
    Sprite sprite;
    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health = 0;
        OnActivate();
        DropParts();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.PlaySound(activateSound);
        if (other.GetComponent<Player>()) OnActivate();
    }
    virtual public void OnActivate() {
        GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(GetComponent<Collider2D>());
    }
}
