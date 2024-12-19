using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float strenght;
    [SerializeField] float fallingSpeed = 5;
    [SerializeField] float floatingSpeed = 0.5f;
    Transform followTarget;
    Vector3 previousPosition;
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 4);
        previousPosition = transform.position;
        followTarget = Camera.main.transform;
    }

    void Update()
    {
        Vector2 difference = followTarget.position - previousPosition;

        transform.position = new Vector2(transform.position.x, transform.position.y + difference.y * strenght);
        previousPosition = followTarget.position;

        transform.position = new Vector2(transform.position.x + floatingSpeed * strenght, transform.position.y - fallingSpeed * strenght);
    }
}
