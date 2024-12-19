using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator)),RequireComponent(typeof(Rigidbody2D))]
public class Boss : Enemy
{
    [Space(5),SerializeField] protected Sprite bossIcon;

    [SerializeField] protected int attackCount = 1;

    [SerializeField] protected float minTimer = 1;
    [SerializeField] protected float maxTimer = 3;

    protected float timerAttack;
    protected int stage = 1;
    
    protected Animator anim;
    protected Boosters booster;
    protected TextMeshProUGUI healthText;
    protected Rigidbody2D rb;

    public virtual void Start()
    {
        GameManager.onBossSpawn.Invoke();
        timerAttack = Random.Range(minTimer,maxTimer);
        GameManager.onRestartGame.AddListener(Kill);
        floatingCrit = Resources.Load<GameObject>("Prefabs/floatingCrit");
        GameManager.canGenerate = false;
        healthBar = GameManager.healthBar;
        healthBar.gameObject.SetActive(true);
        healthText = healthBar.GetComponentInChildren<TextMeshProUGUI>();
        GameObject.Find("BossIcon").GetComponent<Image>().sprite = bossIcon;
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $"{healthBar.maxValue}/{health}"; 
        
        booster = Resources.Load<BoosterPlatformSpawn>("Prefabs/Boosters");
        Invoke(nameof(SpawnArea),0.7f);
    }

    void SpawnArea() => Instantiate(booster,GameManager.Instance.player.transform.position, Quaternion.identity).OnActivate();
    
    public virtual void FixedUpdate()
    {
        if (timerAttack <= 0) {
            DoRandomAttack();
            timerAttack = Random.Range(minTimer,maxTimer);
        } else {
            timerAttack -= Time.fixedDeltaTime;
        }        
    }
    public virtual void DoRandomAttack() {

    }

    public override void TakeDamage(int damage)
    {
        StartCoroutine(Blink());
        if (damageParticles != null) Instantiate(damageParticles, transform.position,Quaternion.identity);
        var newDamage = damage - ((float)damage / 100 * armor); //Применение поглощения урона:000
        if (newDamage <= 0) newDamage = 1;
    
        if (floatingText != null) Instantiate(floatingText, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(-0.5f,0.5f)), Quaternion.identity).GetComponentInChildren<TextMeshPro>().text = Mathf.Round(newDamage).ToString();

        health -= (int)newDamage;
        if (health <= 0) {
            health = 0; 
            KillAnim();
        }
        healthBar.value = health;
        healthText.text = $"{healthBar.maxValue}/{health}"; 
    }
    public virtual void KillAnim() {
        anim.SetTrigger("kill");
    }
    public override void Kill()
    {
        if (!GameManager.Instance.gameIsLoosedOrStoped) {
            var spawnPos = new Vector2(0, Camera.main.transform.position.y - 6);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y - 3);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y + 0);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y + 3);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y + 6);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y + 9);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
            spawnPos = new Vector2(0, Camera.main.transform.position.y + 12);
            Instantiate(booster, spawnPos, Quaternion.identity).OnActivate();
        }
        GameManager.canGenerate = true;
        healthBar.gameObject.SetActive(false);

        foreach (var p in slisedSides)
        {
            Rigidbody2D newPart = Instantiate(p, transform.position,Quaternion.identity);
            newPart.velocity = new Vector2(Random.Range(-MaxForcePart,MaxForcePart),MaxForcePart);
            newPart.rotation = Random.Range(-MaxRotationForcePart,MaxRotationForcePart);
            Destroy(newPart.gameObject,4);
        }
        CameraShake.singleton.Shake(0.3f,4);

        if (killParticles != null) Instantiate(killParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
