using UnityEngine;

public class ShealdBooster : Boosters
{
    [SerializeField] int Duration = 20;
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
        GameManager.Instance.player.TurnOnSheald(Duration);
    }
}
