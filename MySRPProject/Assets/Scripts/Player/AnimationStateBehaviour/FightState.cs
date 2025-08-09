using UnityEngine;

public class FightState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PlayerAnimationEvents>().OnFightStart();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Called even if animation was interrupted
        animator.gameObject.GetComponent<PlayerAnimationEvents>().OnFightEnd();
    }
}
