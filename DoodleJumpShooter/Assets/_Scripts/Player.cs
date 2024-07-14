using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float jumpForce = 8;
    [SerializeField] float springForce = 10;
    [SerializeField] float capSpeed = 5;
    [SerializeField] float capDuration = 2.2f;
    [SerializeField] float rocketSpeed = 7;
    [SerializeField] float rocketDuration = 3.5f;    
    [SerializeField] GameObject virtualCamera;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] Image fill;
    [SerializeField] Image fillChildren;
    [HideInInspector] public Rigidbody2D rb;
    float movement;
    public bool Undieing {get; private set;}  
    bool facingRight = true;
    bool aksEnabled;
    GameObject cap;
    GameObject rocket;
    Coroutine flyProcess;

    void Start()
    {
        virtualCamera.GetComponent<CameraController>().follow = this;
        rb = GetComponent<Rigidbody2D>();
        cap = Resources.Load<GameObject>("Prefabs/A_Cap");
        rocket = Resources.Load<GameObject>("Prefabs/A_Rocket");
        GameManager.onRestartGame.AddListener(OnRestartGame);
    }

    void Update()
    {
        Movement();
    }
    void Movement() {
        movement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * movement, rb.velocity.y);
        if (movement > 0 && !facingRight || movement < 0 && facingRight) Flip(); 
    }
    void Jump() {
        Instantiate(jumpParticles,transform.position, Quaternion.identity);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Flip() {
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
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
                other.GetComponent<Animator>().SetTrigger("Activate");
                StartCoroutine(TurnOnUndieing(2f));
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.7f);
            } else if (other.CompareTag("Spike")) {
                GameManager.Instance.Lose();
            }
        }
        if (!aksEnabled) {
            if (other.CompareTag("Cap")) {
                flyProcess = StartCoroutine(Fly(capDuration, capSpeed));
                StartCoroutine(DressAks(capDuration, cap));
                StartCoroutine(TurnOnUndieing(capDuration));
            Destroy(other.gameObject);
            } else if (other.CompareTag("Rocket")) {
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

            if (!aksEnabled) Destroy(aksessuar);

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

    void OnRestartGame() {
        aksEnabled = false;
        if (flyProcess != null) StopCoroutine(flyProcess);
    }
}
