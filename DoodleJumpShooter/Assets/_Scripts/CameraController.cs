using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player follow;

    void Update()
    {
        if (follow.transform.position.y > transform.position.y) {
            transform.position = new Vector3(transform.position.x,follow.transform.position.y,transform.position.z);
        }
    }
}
