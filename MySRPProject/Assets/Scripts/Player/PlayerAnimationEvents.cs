using Player;
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

    // FightState : StateMachineBehaviour (OnStateExit metoda) skripta koja je dodana na FightState u Animator prozoru poziva ovaj event
    // kada state zavrsi. Pouzdanije rjesenje nego Animation eventi. Omogucuje brze ponavljanje animacije (brzi napad u ovom slucaju).
    // I ne blokira se kao sto eventi rade - ne uspiju se izvrsiti ponekad.
    public void OnFightEnd()
    {
        SlashEvent?.Invoke(false);
    }
}
