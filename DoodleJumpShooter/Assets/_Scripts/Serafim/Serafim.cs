using UnityEngine;

public class Serafim : Boss
{
    [SerializeField] float speed = 2;
    public void Update()
    {
        var directionToPlayer = GameManager.Instance.player.transform.position - transform.position ;
        rb.velocity = directionToPlayer.normalized * speed;
    }
}
