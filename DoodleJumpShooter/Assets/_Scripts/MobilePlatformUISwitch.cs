using System.Collections.Generic;
using UnityEngine;

public class MobilePlatformUISwitch : MonoBehaviour
{
    [SerializeField] List<GameObject> mobile; 

    void Start()
    {
        if (!Application.isMobilePlatform) {
            foreach (var mobileObj in mobile) {
                mobileObj.SetActive(false);
            }
        } 
        Destroy(gameObject);
    }
}
