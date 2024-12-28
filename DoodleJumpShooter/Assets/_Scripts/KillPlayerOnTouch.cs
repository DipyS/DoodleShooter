using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>()) {
            GameManager.Instance.player.TakeDamage(1);
        }
    }
}
