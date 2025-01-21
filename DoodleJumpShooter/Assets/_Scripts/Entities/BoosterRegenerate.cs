using UnityEngine;

public class BoosterRegenerate : Boosters
{
    [SerializeField] int healthCount = 1;
    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health = 0;
        DropParts();
        Kill();
    }
    public override void OnActivate()
    {
        base.OnActivate();
        GameManager.Instance.player.Regenerate(healthCount);
    }
}
