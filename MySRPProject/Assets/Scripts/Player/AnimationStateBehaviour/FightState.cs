using UnityEngine;

public class FightState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Called even if animation was interrupted
        animator.gameObject.GetComponent<PlayerAnimationEvents>().OnFightEnd();
    }
}
