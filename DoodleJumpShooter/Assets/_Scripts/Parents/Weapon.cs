using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] bool automaticShooter;
    [SerializeField] protected float ShootIntervall = 0.7f;
    [SerializeField] protected float targetingOffset;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Rigidbody2D Gilze;
    [SerializeField] protected float knockbackForce = 8;
    protected Player player;
    protected float timerToShoot;
    Animator anim;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        timerToShoot = ShootIntervall;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        timerToShoot -= Time.deltaTime;
        transform.position = player.transform.position;
        RotateGun();
        if (!GameManager.Instance.gameIsLoosedOrStoped && timerToShoot <= 0) {
            if (automaticShooter) {
                if (Input.GetMouseButton(0)) {
                    VirtualShoot();
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    VirtualShoot();
                }
            }
        }
    }
    void VirtualShoot() {
        if (Gilze != null) {
            var newGilze = Instantiate(Gilze,transform.position,Quaternion.identity);
            newGilze.velocity = new Vector2(Random.Range(-4,4),5);
            newGilze.AddTorque(Random.Range(-1000f,1000f));
            Destroy(newGilze.gameObject,5);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 knockbackDirection = new Vector2(transform.position.x - mousePos.x,transform.position.y - mousePos.y);
        
        player.rb.AddForce(new Vector2(player.rb.velocity.x, knockbackDirection.normalized.y * knockbackForce),ForceMode2D.Impulse);
        CameraShake.singleton.Shake(0.3f,5);
        timerToShoot = ShootIntervall;
        Shoot();
    }
    void RotateGun() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 differencePos = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
        float angle = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle + 180);

        if (mousePos.x > transform.position.x) spriteRenderer.flipY = false;
        else if (mousePos.x < transform.position.x) spriteRenderer.flipY = true; 
   }
    protected virtual void Shoot() {

    }
}
