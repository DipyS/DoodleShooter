using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] bool automaticShooter;
    [SerializeField] bool enableLazer;

    [SerializeField] protected float ShootIntervall = 0.7f;
    [SerializeField] protected float targetingOffset;
    [SerializeField] protected float knockbackForce = 8;

    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Rigidbody2D Gilze;

    //СОрян чуваки отвлекся!
    [SerializeField] FixedJoystick ShotJoystick;
    [SerializeField] float directionMultyplayer = 4;
    protected float timerToShoot;
    protected Player player;
    Animator anim;
    SpriteRenderer spriteRenderer;
    LineRenderer line;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        timerToShoot = ShootIntervall;
        player = GameObject.Find("Player").GetComponent<Player>();
        ShotJoystick  = ShotJoystick ?? GameObject.Find("ShotJoystick").GetComponent<FixedJoystick>();
        line = Resources.Load<LineRenderer>("Prefabs/Gilzes/Lazer");
        if (enableLazer) line = Instantiate(line, FirePoint);
    }
    void Update()
    {
        timerToShoot -= Time.deltaTime;
        transform.position = player.transform.position;

        if (!GameManager.Instance.gameIsLoosedOrStoped && timerToShoot <= 0) {
            Vector2 mousePos;

            //На телефоне
            if (Application.isMobilePlatform) {
                mousePos =  new Vector2(ShotJoystick.Direction.x * directionMultyplayer + transform.position.x, ShotJoystick.Direction.y * directionMultyplayer + transform.position.y);
                RotateGun(mousePos);
                
                if (ShotJoystick.Direction.x >= 0.5 || ShotJoystick.Direction.x <= -0.5 || ShotJoystick.Direction.y >= 0.5 || ShotJoystick.Direction.y <= -0.5) {
                    VirtualShoot(mousePos);
                }
            } 
            else {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos);

                if (automaticShooter) {
                    if (Input.GetMouseButton(0)) {
                        VirtualShoot(mousePos);
                    }
                } 
                
                else {
                    if (Input.GetMouseButtonDown(0)) {
                        VirtualShoot(mousePos);
                    }
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
    void VirtualShoot(Vector2 shotDirection) {
        if (Gilze != null) {
            var newGilze = Instantiate(Gilze,transform.position,Quaternion.identity);
            newGilze.velocity = new Vector2(Random.Range(-4,4),5);
            newGilze.AddTorque(Random.Range(-1000f,1000f));
            GameManager.objects.Add(newGilze.gameObject);
            Destroy(newGilze.gameObject,5);
        }

        Vector2 knockbackDirection = new Vector2(transform.position.x - shotDirection.x,transform.position.y - shotDirection.y);
        
        player.rb.AddForce(new Vector2(player.rb.velocity.x, knockbackDirection.normalized.y * knockbackForce),ForceMode2D.Impulse);
        CameraShake.singleton.Shake(0.3f,5);
        timerToShoot = ShootIntervall;
        Shoot();
    }
    void RotateGun(Vector2 RotateDirection) {
        Vector2 differencePos = new Vector2(transform.position.x - RotateDirection.x, transform.position.y - RotateDirection.y);
        float angle = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle + 180);

        if (RotateDirection.x > transform.position.x) spriteRenderer.flipY = false;
        else if (RotateDirection.x < transform.position.x) spriteRenderer.flipY = true; 
   }
    protected virtual void Shoot() {

    }
}
