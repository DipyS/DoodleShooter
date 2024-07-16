using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] bool automaticShooter;
    [SerializeField] bool enableLazer;
    [SerializeField] protected float ShootIntervall = 0.7f;
    [SerializeField] protected float targetingOffset;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Rigidbody2D Gilze;
    [SerializeField] protected float knockbackForce = 8;
    protected Player player;
    protected float timerToShoot;
    Animator anim;
    SpriteRenderer spriteRenderer;
    LineRenderer line;
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        timerToShoot = ShootIntervall;
        player = GameObject.Find("Player").GetComponent<Player>();
        line = Resources.Load<LineRenderer>("Prefabs/Gilzes/Lazer");
        if (enableLazer) line = Instantiate(line, FirePoint);
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

        if (enableLazer) {
            RaycastHit2D hit = Physics2D.Raycast(FirePoint.position, FirePoint.right, 90);
            
            line.SetPosition(0,FirePoint.position);
            if (hit.point == Vector2.zero) {
                Vector2 endPoint = FirePoint.position + FirePoint.right * 90; 
                line.SetPosition(1,endPoint);
            } 
            else line.SetPosition(1,hit.point);
        }
    }
    void VirtualShoot() {
        if (Gilze != null) {
            var newGilze = Instantiate(Gilze,transform.position,Quaternion.identity);
            newGilze.velocity = new Vector2(Random.Range(-4,4),5);
            newGilze.AddTorque(Random.Range(-1000f,1000f));
            GameManager.objects.Add(newGilze.gameObject);
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
