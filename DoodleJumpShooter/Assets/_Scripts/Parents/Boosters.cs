using UnityEngine;

public class Boosters : Entity
{
    public override void TakeDamage(int damage)
    {
        OnActivate();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()) OnActivate();
    }
    virtual public void OnActivate() {

    }
}
