using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int health;
    public int Health {
        get {return health;}
        private set {health = value; VisualizeHealth();}
    }
    [SerializeField] int startHealth = 3;
    [SerializeField] float speed = 4;
    [SerializeField] float jumpForce = 8;
    [SerializeField] float DashForce = 3;
    [SerializeField] float DashDuration = 0.3f;
    [SerializeField] float DashIntervall = 1.2f;
    [SerializeField] float capSpeed = 5;
    [SerializeField] float capDuration = 2.2f;
    [SerializeField] float rocketSpeed = 7;
    [SerializeField, Space(10)] float rocketDuration = 3.5f;   

    [SerializeField] GameObject virtualCamera;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] ParticleSystem dashParticles;
    [SerializeField] ParticleSystem damageParticles;

    [SerializeField, Space(10)] Image fill;
    [SerializeField] Image fillChildren;
    [SerializeField] VariableJoystick movementJoystick;

    [SerializeField, Space(10)] AudioClip jumpSound;
    [SerializeField] AudioClip rocketSound;
    [SerializeField] AudioClip capSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip springSound;

    [SerializeField, Space(10)] HorizontalLayoutGroup group;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] Image heart;
    List<GameObject> hearts = new List<GameObject>();

    float movement;
    float dashTimer;

    public bool Undieing {get; private set;}  
    bool facingRight = true;
    bool aksEnabled;

    GameObject cap;
    GameObject rocket;
    Coroutine flyProcess;
    Animator anim;
    SpriteRenderer spRenderer;
    Coroutine undiyng;

    void Start()
    {
        Health = startHealth;
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
        dashTimer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space)) {
                VirtualDash();
            }
    }

    public void VirtualDash() {
        if (dashTimer <= 0 && !aksEnabled) {
            anim.SetTrigger("Dash");
            StartCoroutine(Dashing());
            dashTimer = DashIntervall;
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
        dashTimer = DashIntervall;

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
        if (rb.velocity.x <= speed && rb.velocity.x >= -speed)
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
                StartCoroutine(TurnOnUndieing(2f));
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
                StartCoroutine(TurnOnUndieing(capDuration));
            Destroy(other.gameObject);
            } else if (other.CompareTag("Rocket")) {
                GameManager.Instance.PlaySound(rocketSound,5);
                flyProcess = StartCoroutine(Fly(rocketDuration, rocketSpeed));
                StartCoroutine(DressAks(rocketDuration, rocket));
                StartCoroutine(TurnOnUndieing(rocketDuration));
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
    IEnumerator TurnOnUndieing(float duration) {
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
        Instantiate(damageParticles, transform.position, Quaternion.identity);
        Health -= damage;
        StartCoroutine(TurnOnUndieing(3));

        if (Health <= 0) {
            GameManager.Instance.Lose();
        }
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
    }

    void OnRestartGame() {
        Health = startHealth;
        StopFly();
    }
}
