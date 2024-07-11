using System.Collections;
using UnityEngine;

public class floatingPlatform : Platform
{
    [SerializeField] float speed;

    void Start()
    {
        StartCoroutine(Float());
    }
    IEnumerator Float() {
        while (transform.position.x > GameManager.Instance.startSpawnPos.x) 
        {
            transform.position = new Vector2(transform.position.x - speed, transform.position.y);
            yield return new WaitForSeconds(0.02f);
        }
        while (transform.position.x < GameManager.Instance.endSpawnPos.x) 
        {
            transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine(Float());
    }
}
