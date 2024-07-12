using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    public List<GameObject> carouselObjects;

    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float StartRadius = 1;
    [SerializeField] float ExpanshionSpeed = 0.05f;
    [SerializeField] float timer;
    [SerializeField] float YOffset;
    float Expanshion;
    float Radius = 0;

    void FixedUpdate()
    {
        Expanshion += ExpanshionSpeed;
    }

    void Update()
    {
        Radius = Mathf.Lerp(0,StartRadius, Expanshion);
        timer += Time.deltaTime * rotationSpeed;
        float angle = 0;
        float offset = 360 / carouselObjects.Count;
        for (int i = 0; i < carouselObjects.Count; i++) {
            angle += offset;
            carouselObjects[i].transform.position = new Vector2(Mathf.Sin((angle + timer) * Mathf.Deg2Rad) * Radius + transform.position.x, Mathf.Cos((angle + timer) * Mathf.Deg2Rad) * Radius + transform.position.y + YOffset);
        }
    }
}
