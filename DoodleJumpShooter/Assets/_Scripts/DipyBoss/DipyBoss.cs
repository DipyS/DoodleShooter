using TMPro;
using UnityEngine;

public class DipyBoss : Boss
{
    [SerializeField, Space( 8)] GameObject Lazer;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject Minion;
    [SerializeField] GameObject SecondStageObject;

    [SerializeField] ParticleSystem ShotBulletParticles;
    [SerializeField] ParticleSystem ShotParticles;

    [SerializeField] Transform LeftHandShotPoint;
    [SerializeField] Transform RightHandShotPoint;
    [SerializeField] Transform Body;

    BoxCollider2D boxCollider2D;
    Vector2 BodyPos {
        get { return new Vector2(Body.position.x - transform.position.x, Body.position.y - transform.position.y);}
    }

    public override void Start()
    {
        base.Start();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        boxCollider2D.offset = BodyPos;
    }

    public void SecondStage() {
        if (SecondStageObject != null) Instantiate(SecondStageObject, Body);
    }

    public override void DoRandomAttack()
    {
        int attackID = Random.Range(0, attackCount + 1);
        anim.SetInteger("Attack", attackID);
        Invoke("ResetAttack", 0.25f);
    }
    
    void ResetAttack() {
        anim.SetInteger("Attack", 0);
    }

    public void LazerLeftHand() {
        var newLazer = Instantiate(Lazer, LeftHandShotPoint.position, LeftHandShotPoint.rotation);
        CameraShake.singleton.Shake(0.3f,5);
    }
    public void LazerRightHand() {
        var newLazer = Instantiate(Lazer, RightHandShotPoint.position, RightHandShotPoint.rotation);
        CameraShake.singleton.Shake(0.3f,5);
    }
    public void BulletLeftHand() {
        var newBullet = Instantiate(Bullet, LeftHandShotPoint.position, LeftHandShotPoint.rotation);
        Instantiate(ShotBulletParticles, LeftHandShotPoint.position, LeftHandShotPoint.rotation);
        CameraShake.singleton.Shake(0.3f,5);
    }
    public void BulletRightHand() {
        var newBullet = Instantiate(Bullet, RightHandShotPoint.position, RightHandShotPoint.rotation);
        Instantiate(ShotBulletParticles, RightHandShotPoint.position, RightHandShotPoint.rotation);
        CameraShake.singleton.Shake(0.3f,5);
    }
    public override void TakeDamage(int damage)
    {
        StartCoroutine(Blink());
        if (damageParticles != null) Instantiate(damageParticles, BodyPos,Quaternion.identity);
        var newDamage = damage - ((float)damage / 100 * armor); //Применение поглощения урона:000
        if (newDamage <= 0) newDamage = 1;

        if (floatingText != null) Instantiate(floatingText, new Vector2(BodyPos.x + Random.Range(-0.5f,0.5f),BodyPos.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = Mathf.Round(newDamage).ToString();

        health -= (int)newDamage;
        if (health <= 0) KillAnim();

        healthBar.value = health;

        
        if (health <= healthBar.maxValue / 100 * 45 && stage == 1) 
        {
            stage = 2;
            armor += 20;
            anim.SetTrigger("stage2");
            minTimer -= 1;
            maxTimer -= 1;
        }
    }
    public override void Kill()
    {
        base.Kill();
        Destroy(gameObject);
    }
}
