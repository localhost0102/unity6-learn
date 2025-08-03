using Player;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void OnWalkingAnimationEnded()
    {
        AnimationController.SetWalkingEnded();
    }

    
}
