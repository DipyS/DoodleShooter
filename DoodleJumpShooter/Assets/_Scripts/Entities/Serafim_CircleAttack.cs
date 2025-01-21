using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serafim_CircleAttack : StateMachineBehaviour
{
    [SerializeField] float rotationSpeed = 20;
    [SerializeField] float attackRadius = 4;
    [SerializeField] float attackDelay = 1;
    [SerializeField] EnemyBullet bullet;
    [SerializeField] float shotIntervall = 0.1f;
    [SerializeField] int shotCount = 5;
    GameObject gameObject;
    float sinValue;
    float cosValue;
    float delayTimer;
    int currentShotCount;
    float shotTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delayTimer = attackDelay;
        gameObject = animator.gameObject;
        sinValue = Random.Range(0,361);
        cosValue = sinValue;
        currentShotCount = 0;
        shotTimer = shotIntervall;
        CameraController.secondFollow = GameManager.Instance.player.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (delayTimer <= 0) {
            sinValue += rotationSpeed * Time.deltaTime;
            cosValue += rotationSpeed * Time.deltaTime;

            Vector2 movePosition = new Vector2(Mathf.Sin(sinValue) * attackRadius + GameManager.Instance.player.transform.position.x, Mathf.Cos(cosValue) * attackRadius + GameManager.Instance.player.transform.position.y);
            gameObject.transform.position = movePosition;

            if (shotTimer <= 0 && currentShotCount < shotCount) {
                currentShotCount++;
                shotTimer = shotIntervall;

                var newBullet = Instantiate(bullet, gameObject.transform.position,Quaternion.identity);
                Vector2 difference = new Vector2(gameObject.transform.position.x - GameManager.Instance.player.transform.position.x, gameObject.transform.position.y - GameManager.Instance.player.transform.position.y);
                float Angle = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;  
                newBullet.transform.rotation = Quaternion.Euler(0,0,Angle);
            } else {
                shotTimer -= Time.deltaTime;
            }
        } else {
            delayTimer -= Time.deltaTime;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CameraController.secondFollow = gameObject.transform;
    }
}
