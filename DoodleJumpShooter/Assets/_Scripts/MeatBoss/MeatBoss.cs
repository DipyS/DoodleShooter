using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBoss : Boss
{
    [SerializeField] List<GameObject> minion;
    [SerializeField] GameObject bullet;
    [SerializeField] int onDamageMinionSpawnChance = 4;
    [SerializeField] int shootCount;
    [SerializeField] float OffsetAngle;
    [SerializeField] byte attackCount;
    Boosters booster;
    int stage = 1;
    public override void Start()
    {
        base.Start();
        booster = Resources.Load<BoosterPlatformSpawn>("Prefabs/Boosters");
        Instantiate(booster,GameManager.Instance.player.transform.position, Quaternion.identity).OnActivate();
    }
    void Update()
    {
        if (health <= healthBar.maxValue / 100 * 45 && stage == 1) 
        {
            stage = 2;
            armor += 20;
            anim.SetTrigger("stage2");
            onDamageMinionSpawnChance += 20;
            minTimer -= 1;
            maxTimer -= 1;
        }
    }
    public override void DoRandomAttack()
    {
        int attackID = Random.Range(0, attackCount + 1);
        switch (attackID) {
            case 1: anim.SetTrigger("SquashShoot"); break;
            case 2: anim.SetTrigger("ShotOnPlace1"); break;
            case 3: anim.SetTrigger("ShotOnPlace2"); break;
        }
    }
    public void ShootOnPlayer() {
        Vector2 differencePos = new Vector2(transform.position.x - GameManager.Instance.player.transform.position.x, transform.position.y - GameManager.Instance.player.transform.position.y);
        float angle = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;
        
        Instantiate(bullet,transform.position,Quaternion.Euler(0, 0, angle + Random.Range(-OffsetAngle / 2, OffsetAngle / 2)));
    }

    public void SquashShoot() {
        for (int i = 0; i < shootCount; i++) {
            ShootOnPlayer();
        }
    }

    public override void TakeDamage(int damage)
    {
        int chance = Random.Range(1,101);
        if (chance <= onDamageMinionSpawnChance) 
        {
            InstanceMinion();
        }
        base.TakeDamage(damage);
    }

    public void InstanceMinion(int count = 1)
    {   
        for (int i = 0; i < count; i++) {
            if (minion != null)
            {
                Instantiate(minion[Random.Range(0,minion.Count)], transform.position, Quaternion.identity);
            }
        }
    }

    public override void Kill()
    {
        var spawnPos = new Vector2(transform.position.x, transform.position.y - 5);
        Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
        spawnPos = new Vector2(transform.position.x, transform.position.y - 2);
        Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();

        base.Kill();
        Destroy(gameObject);
    }
}
