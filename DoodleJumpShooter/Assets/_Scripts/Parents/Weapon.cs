using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] bool automaticShooter;
    [SerializeField] protected float ShootIntervall = 0.7f;
    [SerializeField] protected float targetingOffset;
    [SerializeField] protected Transform FirePoint;
    protected Player player;
    protected float timerToShoot;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        timerToShoot = ShootIntervall;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        timerToShoot -= Time.deltaTime;
        transform.position = player.transform.position;
        RotateGun();
        
        if (!GameManager.Instance.gameIsLoosedOrStoped) {
            if (automaticShooter) {
                if (Input.GetMouseButton(0)) {
                    timerToShoot = ShootIntervall;
                    CameraShake.singleton.Shake(0.3f,5);
                    Shoot();
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    CameraShake.singleton.Shake(0.3f,5);
                    timerToShoot = ShootIntervall;
                    Shoot();
                }
            }
        }
    }
    void RotateGun() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 differencePos = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
        float angle = Mathf.Atan2(differencePos.y, differencePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle + 180);
    }
    protected virtual void Shoot() {

    }
}
