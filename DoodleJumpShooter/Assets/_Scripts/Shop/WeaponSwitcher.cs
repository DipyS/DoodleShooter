using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public static WeaponSwitcher singleton;

    void Awake()
    {
        singleton = this;    
    }
    public void SwitchWeaponTo(string Name) {
        foreach (Transform weapon in transform)
        {
            if (weapon.name == Name) weapon.gameObject.SetActive(true);
            else weapon.gameObject.SetActive(false);
        }
    }
    public void SwitchWeaponTo() {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }
}
