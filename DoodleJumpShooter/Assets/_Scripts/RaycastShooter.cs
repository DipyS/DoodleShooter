using UnityEngine;
using System.Collections;

public class RaycastShooter : Weapon
{
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem shotParticles;
    [SerializeField] float intervallToShoot;
    [SerializeField] int shootCount;
    [SerializeField] float shootDistance;
    [SerializeField] LayerMask ignoreRaycast;
    [SerializeField] int damage = 15;
    [SerializeField,Space(5)] bool VisualizeBullet;
    [SerializeField] LineRenderer bullet;
    protected override void Shoot()
    {
        StartCoroutine(Shooting());
    }
    IEnumerator Shooting() {
        for (int i = 0; i < shootCount; i++) {
            timerToShoot = float.MaxValue;
            
            Quaternion firePointDefauldRotation = FirePoint.rotation;
            FirePoint.Rotate(0,0,Random.Range(-targetingOffset,targetingOffset));

            var newShotParticles = Instantiate(shotParticles,FirePoint.position,FirePoint.rotation);
            newShotParticles.transform.Rotate(-90,0,0);

            RaycastHit2D hit = Physics2D.Raycast(FirePoint.position, FirePoint.right, shootDistance,~ignoreRaycast);
            if (hit.collider != null) {
                Instantiate(hitParticles, hit.point, Quaternion.identity);
                if (hit.collider.TryGetComponent(out Entity entity)) {
                    entity.TakeDamage(damage);
                }
            }
            
            if (VisualizeBullet) {
                var newBullet = Instantiate(bullet,transform.position,Quaternion.identity);
                newBullet.SetPosition(0,FirePoint.position);
                if (hit.point == Vector2.zero) {
                    Vector2 endPoint = FirePoint.position + FirePoint.right * 90; 
                    newBullet.SetPosition(1,endPoint);
                } 
                else newBullet.SetPosition(1,hit.point);
            }
            FirePoint.rotation = firePointDefauldRotation;
            if (intervallToShoot != 0) yield return new WaitForSeconds(intervallToShoot);
        }
        timerToShoot = ShootIntervall;
    }
}

