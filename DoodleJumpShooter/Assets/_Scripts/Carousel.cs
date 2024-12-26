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
    [SerializeField] protected float sinMultiplier = 1;
    [SerializeField] protected float cosMultiplier = 1;
    [SerializeField] protected float sinAdd;
    [SerializeField] protected float cosAdd;
    float Expanshion;
    float Radius = 0;

    public void FixedUpdate()
    {
        Expanshion += ExpanshionSpeed;
    }

    public void Update()
    {
        var removeObjects = new List<GameObject>();
        foreach (var obj in carouselObjects) {
            if (obj == null) removeObjects.Add(obj);
        }
        foreach (var obj in removeObjects) {
            carouselObjects.Remove(obj);
        }
        if (carouselObjects.Count <= 0) {
            Destroy(gameObject);
            return;
        }
        Radius = Mathf.Lerp(0,StartRadius, Expanshion);
        timer += Time.deltaTime * rotationSpeed;
        float angle = 0;
        float offset = 360 / carouselObjects.Count;
        for (int i = 0; i < carouselObjects.Count; i++) {
            angle += offset;
            carouselObjects[i].transform.position = new Vector2(Mathf.Sin((angle + timer + sinAdd) * Mathf.Deg2Rad * sinMultiplier) * 
            Radius + transform.position.x, Mathf.Cos((angle + timer + cosAdd) * Mathf.Deg2Rad * cosMultiplier) * Radius + transform.position.y + YOffset);
        }
    }
}
