using UnityEngine;

public class Enemy : Entity
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().Undieing) {
            GameManager.Instance.Lose();
        }
    }
}
