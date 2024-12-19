using System.Collections;
using UnityEngine;

public class PrefabShooter : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float intervallToShoot;
    public int shootCount;

    protected override void Shoot()
    {
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting() {
        for (int i = 0; i < shootCount; i++) {
            GameManager.Instance.PlaySound(shotSound);
            timerToShoot = float.MaxValue;
            GameObject newBullet = Instantiate(bulletPrefab, FirePoint.position,FirePoint.rotation);
            newBullet.transform.Rotate(0,0,Random.Range(-targetingOffset, targetingOffset));
            if (intervallToShoot != 0) yield return new WaitForSeconds(intervallToShoot);
        }
        timerToShoot = ShootIntervall;
    }
}
