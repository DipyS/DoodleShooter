using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public void DestroyObject() {
        Destroy(gameObject);
    }
    public void DestroyParent() {
        Destroy(transform.parent);
    }
}
