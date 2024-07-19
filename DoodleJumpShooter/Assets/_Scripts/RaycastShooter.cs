using UnityEngine;
using System.Collections;

public class RaycastShooter : Weapon
{
    [SerializeField, Space(4)] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem shotParticles;

    [SerializeField, Space(4)] LayerMask ignoreRaycast;
    [SerializeField] LineRenderer bullet;

    [SerializeField, Space(4)] int shootCount;
    [SerializeField] int damage = 15;

    [SerializeField, Space(4)] float intervallToShoot;
    [SerializeField] float shootDistance;
    [SerializeField] float autoTargettingAngle = 5;
    [SerializeField] float autoTargettingCheckIntervall = 1.5f;
    
    [SerializeField, Space(4)] bool VisualizeBullet;
    [SerializeField] bool autoTargetting = true;
    
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
            
            //Auto Targetting
            if (hit.collider == null && autoTargetting) {
                float currentAngle = autoTargettingCheckIntervall;
                Quaternion ShotRotation = FirePoint.rotation;
                
                while (currentAngle <= autoTargettingAngle) {

                    FirePoint.Rotate(0, 0, currentAngle);
                    
                    hit = Physics2D.Raycast(FirePoint.position, FirePoint.right, shootDistance,~ignoreRaycast);
                    if (hit.collider != null) break;

                    FirePoint.rotation = ShotRotation;
                    FirePoint.Rotate(0, 0, -currentAngle);

                    hit = Physics2D.Raycast(FirePoint.position, FirePoint.right, shootDistance,~ignoreRaycast);
                    if (hit.collider != null) break;
                    
                    FirePoint.rotation = ShotRotation;
                    
                    currentAngle += autoTargettingCheckIntervall;
                }
            }
            
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

