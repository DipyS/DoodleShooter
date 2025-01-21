using Unity.VisualScripting;
using UnityEngine;

public class Serafim_Idle : StateMachineBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shotIntervall = 0.2f;
    [SerializeField] int shotCount = 15;
    [SerializeField] int attackCount = 2; 
    [SerializeField] AudioClip shotSound;
    GameObject gameObject;
    int shots;
    float shotTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        shots = 0;    
        shotTimer = 1f;
        gameObject = animator.gameObject;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (shotTimer <= 0) {
            
            shotTimer = shotIntervall;
            shots++;
            
            Shot();

            if (shots >= shotCount) {animator.SetTrigger("Attack" + Random.Range(1,shotCount + 1));}
        
        } else {
            shotTimer -= Time.fixedDeltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 1; i <= attackCount; i++)
        {
            animator.ResetTrigger("Attack" + attackCount.ToString());
        }
    }

    void Shot() {
        GameManager.Instance.PlaySound(shotSound);
        var newBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
        newBullet.transform.rotation = Quaternion.Euler(0,0, Random.Range(0,361));
    }
}
