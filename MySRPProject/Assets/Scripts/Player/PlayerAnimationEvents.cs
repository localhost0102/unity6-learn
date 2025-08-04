using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static readonly UnityEvent<string> FootstepEvent = new UnityEvent<string>();
    public static readonly UnityEvent<bool> SlashEvent = new UnityEvent<bool>();
    
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
    
    private void OnSlashAnimationStarted(int slashPosition)
    {
        bool hasStarted = slashPosition == 1;
        SlashEvent?.Invoke(hasStarted);
    }

}
