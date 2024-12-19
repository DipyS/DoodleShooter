using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [HideInInspector] public Weapon SelectedWeapon; 
    public static WeaponSwitcher singleton;

    void Awake()
    {
        SwitchWeaponTo("Pistol");
        singleton = this;    
    }
    public void SwitchWeaponTo(string Name) {
        foreach (Transform weapon in transform)
        {
            if (weapon.name == Name) {
                weapon.gameObject.SetActive(true);
                SelectedWeapon = weapon.GetComponent<Weapon>();
            }
            else weapon.gameObject.SetActive(false);
        }
    }
    public void SwitchWeaponTo() {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    } 

    public void AutomaticalShootSwitch() {
        SelectedWeapon.automaticShooter = !SelectedWeapon.automaticShooter; 
    }

    public void AddKnockBackForce() {
        SelectedWeapon.knockbackForce += 2; 
    }

    public void CutKnockBackForce() {
        SelectedWeapon.knockbackForce -= 2; 
        if (SelectedWeapon.knockbackForce < 0) SelectedWeapon.knockbackForce = 0;
    }

    public void AddShootIntervall() {
        SelectedWeapon.ShootIntervall += 0.8f; 
    }
    public void CutShootIntervall() {
        SelectedWeapon.ShootIntervall -= 0.8f; 
    }
    
    public void AddTargettingOffset() {
        SelectedWeapon.targetingOffset += 5;
        if (SelectedWeapon.targetingOffset > 180) SelectedWeapon.targetingOffset = 180;
    }
    
    public void CutTargettingOffset() {
        SelectedWeapon.targetingOffset += 5;
        if (SelectedWeapon.targetingOffset < 0) SelectedWeapon.targetingOffset = 0;
    }

    public void TryAddShootCount() {
        if (SelectedWeapon.TryGetComponent(out RaycastShooter raycastShooter)) {
            raycastShooter.shootCount++;
        } else if (SelectedWeapon.TryGetComponent(out PrefabShooter prefabShooter)) {
            prefabShooter.shootCount++;
        }
    }
    public void TryCutShootCount() {
        if (SelectedWeapon.TryGetComponent(out RaycastShooter raycastShooter)) {
            raycastShooter.shootCount--;
            if (raycastShooter.shootCount < 0) raycastShooter.shootCount = 0;
        } else if (SelectedWeapon.TryGetComponent(out PrefabShooter prefabShooter)) {
            prefabShooter.shootCount--;
            if (prefabShooter.shootCount < 0) prefabShooter.shootCount = 0;
        }
    }

    public void TryAddAutoTargettingAngle() {
        if (SelectedWeapon.TryGetComponent(out RaycastShooter raycastShooter)) {
            raycastShooter.autoTargettingAngle += 7;
            if (raycastShooter.autoTargettingAngle > 180) raycastShooter.autoTargettingAngle = 180;
        }
    }
    public void TryCutAutoTargettingAngle() {
        if (SelectedWeapon.TryGetComponent(out RaycastShooter raycastShooter)) {
            raycastShooter.autoTargettingAngle -= 7;
            if (raycastShooter.autoTargettingAngle < 0) raycastShooter.autoTargettingAngle = 0;
        }
    }

    public void Lol() {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
