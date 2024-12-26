using UnityEngine;

public class Serafim : Boss
{
    [SerializeField] float dashForce;
    [SerializeField] AudioClip dashSound;
    
    public void Dash() {
        GameManager.Instance.PlaySound(dashSound);
        var directionToPlayer = GameManager.Instance.player.transform.position - gameObject.transform.position;
        rb.velocity = directionToPlayer.normalized * dashForce; 
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (health <= healthBar.maxValue / 100 * 50 && stage == 1) 
        {
            if (SecondStageObject != null) Instantiate(SecondStageObject, transform);
            if (secondStageSound != null) GameManager.Instance.PlaySound(secondStageSound);
            stage = 2;
            armor += 20;
            anim.SetTrigger("stage2");
        }
    }
}
