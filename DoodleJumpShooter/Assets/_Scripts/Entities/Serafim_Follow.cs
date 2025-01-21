using UnityEngine;

public class Serafim_Follow : StateMachineBehaviour
{
    [SerializeField] float speed = 2f;
    GameObject gameObject;
    Rigidbody2D rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       gameObject = animator.gameObject;
       rb = animator.gameObject.GetComponent<Rigidbody2D>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var directionToPlayer = GameManager.Instance.player.transform.position - gameObject.transform.position ;
        rb.velocity = directionToPlayer.normalized * speed; 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero; 
    }
}
