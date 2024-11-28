using UnityEngine;

public class Boosters : Entity
{
    Sprite sprite;
    public override void TakeDamage(int damage)
    {
        OnActivate();
        DropParts();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()) OnActivate();
    }
    virtual public void OnActivate() {
        GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(GetComponent<Collider2D>());
    }
}
