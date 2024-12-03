using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float strenght;
    [SerializeField] float fallingSpeed = 5;
    Transform followTarget;
    Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
        followTarget = Camera.main.transform;
    }

    void Update()
    {
        Vector2 difference = followTarget.position - previousPosition;

        transform.position = new Vector2(transform.position.x, transform.position.y + difference.y * strenght);
        previousPosition = followTarget.position;

        transform.position = new Vector2(transform.position.x, transform.position.y - fallingSpeed * strenght);
    }
}
