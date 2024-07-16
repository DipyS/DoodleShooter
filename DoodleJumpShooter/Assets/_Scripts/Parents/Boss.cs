using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator)),RequireComponent(typeof(Rigidbody2D))]
public class Boss : Enemy
{
    [Space(5),SerializeField] protected Sprite bossIcon;
    protected Slider healthBar;
    protected Animator anim;
    [SerializeField] protected float minTimer = 1;
    [SerializeField] protected float maxTimer = 3;
    protected float timerAttack;

    public virtual void Start()
    {
        timerAttack = Random.Range(minTimer,maxTimer);
        GameManager.onRestartGame.AddListener(Kill);
        GameManager.canGenerate = false;
        healthBar = GameManager.healthBar;
        healthBar.gameObject.SetActive(true);
        GameObject.Find("BossIcon").GetComponent<Image>().sprite = bossIcon;

        anim = GetComponent<Animator>();

        healthBar.maxValue = health;
        healthBar.value = health;
    }
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
        base.TakeDamage(damage);
        healthBar.value = health;
    }
    public override void Kill()
    {
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
        anim.SetTrigger("kill");
    }
}
