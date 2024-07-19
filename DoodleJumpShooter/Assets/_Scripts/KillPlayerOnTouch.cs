using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() && !other.gameObject.GetComponent<Player>().Undieing) {
            GameManager.Instance.Lose();
        }
    }
}
