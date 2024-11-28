using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotToTarget : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform target;
    [SerializeField] float shootIntervall;
    
    void Start()
    {
        target = GameObject.Find("Player").transform;
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting() {
        yield return new WaitForSeconds(shootIntervall);
        var newBullet = Instantiate(bullet,transform.position,Quaternion.identity);
        
        Vector2 difference = new Vector2(transform.position.x - target.position.x, transform.position.y - target.position.y);
        float Angle = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;  
        newBullet.transform.rotation = Quaternion.Euler(0,0,Angle);

        StartCoroutine(Shooting());
    }
}
