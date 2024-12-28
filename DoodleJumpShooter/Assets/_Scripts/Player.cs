using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Player : MonoBehaviour
{
    private int health;
    public int Health {
        get {return health;}
        private set {health = value; VisualizeHealth();}
    }
    public int startHealth = 3;
    [SerializeField] float speed = 4;
    [SerializeField] float jumpForce = 8;
    [SerializeField] float DashForce = 3;
    [SerializeField] float DashDuration = 0.3f;
    [SerializeField] float DashIntervall = 1.2f;
    [SerializeField] float ShealdIntervall = 20;
    [SerializeField] float capSpeed = 5;
    [SerializeField] float capDuration = 2.2f;
    [SerializeField] float rocketSpeed = 7;
    [SerializeField, Space(10)] float rocketDuration = 3.5f;   

    [SerializeField] GameObject virtualCamera;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] ParticleSystem dashParticles;
    [SerializeField] ParticleSystem damageParticles;
    [SerializeField] ParticleSystem shealdDamageParticles;

    [SerializeField, Space(10)] Image fill;
    [SerializeField] Image heart;
    [SerializeField] Image shealdFill;
    [SerializeField] Image fillChildren;
    [SerializeField] VariableJoystick movementJoystick;

    [SerializeField, Space(10)] AudioClip jumpSound;
    [SerializeField] AudioClip rocketSound;
    [SerializeField] AudioClip capSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip springSound;
    [SerializeField] Sprite sheadHearts;

    [SerializeField, Space(10)] HorizontalLayoutGroup group;
    [HideInInspector] public Rigidbody2D rb;
    List<GameObject> hearts = new List<GameObject>();

    float movement;
    float actTimer;

    public bool Undieing {get; private set;}  
    bool facingRight = true;  
    bool isSheald;
    bool aksEnabled;

    GameObject cap;
    GameObject rocket;
    Coroutine flyProcess;
    Coroutine shealdProcess;
    Coroutine undyingProcess;
    Animator anim;
    SpriteRenderer spRenderer;

    public enum ActType
    {
        None,
        Dash,
        Sheald,
        SecondJump
    }

    public ActType actType = ActType.None;
    

    void Start()
    {
        actType = ActType.Dash;
        Health = startHealth;
        shealdFill.gameObject.SetActive(false);
        virtualCamera.GetComponent<CameraController>().follow = this;
        rb = GetComponent<Rigidbody2D>();
        cap = Resources.Load<GameObject>("Prefabs/A_Cap");
        rocket = Resources.Load<GameObject>("Prefabs/A_Rocket");
        movementJoystick ??= GameObject.Find("MovementJoystick").GetComponent<VariableJoystick>();
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        GameManager.onRestartGame.AddListener(OnRestartGame);
        GameManager.onBossSpawn.AddListener(StopFly);
    }

    void Update()
    {
        Movement();
        actTimer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space)) {
            ActButton();
        }
    }

    public void ActButton() {
        if (actTimer > 0) return;
        switch (actType)
        {
            case ActType.Dash:
                if (!aksEnabled) 
                {
                    anim.SetTrigger("Dash");
                    StartCoroutine(Dashing());
                    actTimer = DashIntervall;
                }
            break;
            case ActType.Sheald:
                TurnOnSheald(10);
                actTimer = ShealdIntervall;
            break;
        }
    }

    public void VirtualDash() {
        if (actTimer <= 0 && !aksEnabled) {
            anim.SetTrigger("Dash");
            StartCoroutine(Dashing());
            actTimer = DashIntervall;
        }
    }

    IEnumerator Dashing() {
        GameManager.Instance.PlaySound(dashSound);
        Undieing = true;
        if (facingRight) {
            rb.velocity = new Vector2(DashForce, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(-DashForce, rb.velocity.y);
        }

        Instantiate(dashParticles,transform.position, Quaternion.identity);
        yield return new WaitForSeconds(DashDuration/2);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        Instantiate(dashParticles,transform.position, Quaternion.identity);
        yield return new WaitForSeconds(DashDuration/2);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        Instantiate(dashParticles,transform.position, Quaternion.identity);
        actTimer = DashIntervall;

        rb.velocity = new Vector2(0, 0);
        Undieing = false;
    }
    void Movement() {
        if (Application.isMobilePlatform) {
            movement = movementJoystick.Direction.x;
        }
        else{
            movement = Input.GetAxis("Horizontal");
        } 
        if (movement > 0 && !facingRight || movement < 0 && facingRight) Flip();
        if (rb.velocity.x <= speed + 1 && rb.velocity.x >= -speed - 1)
            rb.velocity = new Vector2(speed * movement, rb.velocity.y);
    }
    void Jump() {
        GameManager.Instance.PlaySound(jumpSound);
        if (movement > 0) anim.SetTrigger("jump2");
        else if (movement < 0) anim.SetTrigger("jump1");

        Instantiate(jumpParticles,transform.position, Quaternion.identity);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Flip() {
        facingRight = !facingRight;
        spRenderer.flipX = !spRenderer.flipX;
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (rb.velocity.y <= 0.2f) {
            if (other.collider.gameObject.GetComponent<Platform>()) {
                Jump();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (rb.velocity.y <= 0.2f) {
            if (other.gameObject.GetComponent<Platform>()) {
                Jump();
            }
        }
        if (rb.velocity.y <= 0) {
            if (other.CompareTag("Spring")) {
                GameManager.Instance.PlaySound(springSound,2);
                other.GetComponent<Animator>().SetTrigger("Activate");
                TurnOnUndieing(2f);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.7f);
            } else if (other.CompareTag("Spike")) {
                TakeDamage(1);
            }
        }
        if (!aksEnabled) {
            if (other.CompareTag("Cap")) {
                GameManager.Instance.PlaySound(capSound,5);
                flyProcess = StartCoroutine(Fly(capDuration, capSpeed));
                StartCoroutine(DressAks(capDuration, cap));
                TurnOnUndieing(capDuration);
            Destroy(other.gameObject);
            } else if (other.CompareTag("Rocket")) {
                GameManager.Instance.PlaySound(rocketSound,5);
                flyProcess = StartCoroutine(Fly(rocketDuration, rocketSpeed));
                StartCoroutine(DressAks(rocketDuration, rocket));
                TurnOnUndieing(rocketDuration);
            Destroy(other.gameObject);
            }
        }
    }

    IEnumerator Fly(float duration, float speed) {
        float timer = 0;
        while (timer <= duration) {
            rb.velocity = new Vector2(rb.velocity.x, speed);

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    IEnumerator DressAks(float duration, GameObject aks) {
        fill.gameObject.SetActive(true);
        fill.fillAmount = 1;
        fillChildren.sprite = aks.GetComponentInChildren<SpriteRenderer>().sprite;

        float timer = 0;
        aksEnabled = true;
        GameObject aksessuar = Instantiate(aks, transform);

        while (timer <= duration) {
            fill.fillAmount = 1 - timer / duration;

            if (!aksEnabled) {
                Destroy(aksessuar);
                fill.gameObject.SetActive(false);
            }
            timer += Time.deltaTime;
            yield return null;

        }
        fill.gameObject.SetActive(false);

        aksEnabled = false;
        Destroy(aksessuar);
    }
    void TurnOnUndieing(float duration) {
        if (undyingProcess != null) StopCoroutine(undyingProcess);

        StartCoroutine(ITurnOnUndieing(duration));
    }

    IEnumerator ITurnOnUndieing(float duration) {
        Undieing = true;
        yield return new WaitForSeconds(duration);
        Undieing = false;
    }

    void StopFly() {
        aksEnabled = false;
        if (flyProcess != null) StopCoroutine(flyProcess);
    }

    public void TakeDamage(int damage) {
        if (Undieing) return;
        if (damage < 0) damage = 0;
        CameraShake.singleton.Shake(0.2f, 6f);
        TurnOnUndieing(2);
        
        if (isSheald) {
            Instantiate(shealdDamageParticles, transform.position, Quaternion.identity);
            TurnOffSheald();
        } else {
            Instantiate(damageParticles, transform.position, Quaternion.identity);
            Health -= damage;

            if (Health <= 0) {
                GameManager.Instance.Lose();
            }
        }
    }

    public void TurnOffSheald() {
        shealdFill.gameObject.SetActive(false);
        isSheald = false;
        VisualizeHealth();
    }
    public void TurnOnSheald(float Duration) {
        if (shealdProcess != null) StopCoroutine(shealdProcess);

        shealdProcess = StartCoroutine(ITurnOnShead(Duration));
    }
    
    private IEnumerator ITurnOnShead(float Duration) {
        isSheald = true;
        VisualizeHealth();
        shealdFill.gameObject.SetActive(true);
        float timer = Duration;

        while (timer > 0) {
            shealdFill.fillAmount = timer / Duration;
            Debug.Log(Duration / timer);

            timer -= Time.deltaTime;
            yield return null;
        }
        
        TurnOffSheald();
    }
    public void Regenerate(int health) {
        if (health < 0) health = 0;
        Instantiate(damageParticles, transform.position, Quaternion.identity);
        Health += health;
    }

    void VisualizeHealth() {
        if (Health < 0) Health = 0;
        if (Health > hearts.Count) {
            while(Health > hearts.Count) {
                var newHeart = Instantiate(heart, group.transform);
                hearts.Add(newHeart.gameObject);
            }
        } 
        else if (Health < hearts.Count) {
            List<GameObject> deletingObjects = new List<GameObject>();
            
            for (int i = 0; i < hearts.Count; i++) {
                if (i > Health - 1) { Destroy(hearts[i].gameObject); deletingObjects.Add(hearts[i]); }
            }

            foreach (var item in deletingObjects)
            {
                hearts.Remove(item);
            }
        }

        foreach (var item in hearts)
        {
            if (isSheald)
            item.GetComponent<Image>().sprite = sheadHearts;
            else
            item.GetComponent<Image>().sprite = heart.GetComponent<Image>().sprite;
        }
    }

    void OnRestartGame() {
        actTimer = 0;
        Health = startHealth;
        StopFly();
    }

    void Load() {
        startHealth = YandexGame.savesData.StartHealth;
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }
}
