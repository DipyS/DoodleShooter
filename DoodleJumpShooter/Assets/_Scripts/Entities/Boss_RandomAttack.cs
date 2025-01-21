using UnityEngine;

public class Boss_RandomAttack : StateMachineBehaviour
{
    [SerializeField] float minDuration = 5; 
    [SerializeField] float maxDuration = 6;
    [SerializeField, Space(10)] int transitionCount = 2; 
    float timer;
    string attackName;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minDuration, maxDuration);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0) {
            timer = Random.Range(minDuration, maxDuration);
            attackName = "Attack" + Random.Range(1, transitionCount + 1).ToString();
            animator.SetTrigger(attackName);
        } else {
            timer -= Time.fixedDeltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger(attackName);
    }
}
