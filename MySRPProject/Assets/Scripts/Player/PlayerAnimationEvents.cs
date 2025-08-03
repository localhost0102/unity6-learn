using Player;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static readonly UnityEvent<string> FootstepEvent = new UnityEvent<string>();
    
    public void OnWalkingAnimationEnded(int step)
    {
        OnWalkingStepEvent(step);
        AnimationController.SetWalkingEnded();
    }

    private void OnWalkingStepEvent(int step)
    {
        if (step == 0) return;

        if (step % 2 != 0)
        {
            FootstepEvent?.Invoke("1");
        }
        else
        {
            FootstepEvent?.Invoke("2");
        }
    }
}
