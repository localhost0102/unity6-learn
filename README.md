# README #

### What is this repository for? ###
#### This repository contains Unity game with various elements which can be used in real game development.

### Some of the examples are:
#### C# design patterns applied on Character controls `(ex. Command pattern)`
   * Controls include: ` walk, jump, attack, walk+attack carry objects`
* Walk + attack introduced through Layered animation steps:
    <ol>
        <li>Make sure you have both anim files</li>
        <li>
            Create Avatar Mask (for Upper Body)
            <ul>
                <li>Go to Project > Right Click > Create > Avatar Mask</li>
            </ul>
            <ul>
                <li>Name it UpperBodyMask</li>
                <li>Select it, and in Inspector uncheck all Body parts except:</li>
                <li>Body, Head, Left Arm, Right Arm, Left Hand, Right Hand</li>
                <li>Save the mask</li>
            </ul>
            <li>
                Set Up Animator Controller (Open your Animator Controller)
                <ul>
                    <li>Add a Walking state and set it as default</li>
                    <li>Make transitions from Idle/other states if needed.</li>
                    <li>Add new Layer (Click + at the top right → Name it Fighting)</li>
                    <li>Set Blending = Override (or Additive depending on desired behavior)</li>
                    <li>Assign the UpperBodyMask you made earlier.</li>
                </ul>
            </li>
        </li>
        <li>
            Inside the Fighting Layer:
            <ul>
                <li>Add the Fighting animation.</li>
                <li>Set it as default, or make a transition to it from an Empty state.</li>
                <li>Add a parameter like isFighting (bool) and condition to control the transition.</li>
            </ul>
        </li>
        <li>
            Rest is up to you...trigger the animation and test everything.
        </li>
    </ol>
    * Improved animation state manipulation with StateMachineBehaviour.
* StateMachineBehaviour:
    * ✅ OnStateExit via StateMachineBehaviour Instead of using Animation Events. More reliable.
    ** Create a StateMachineBehaviour script:
    <pre> public class FightState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
        { 
            // Called even if animation was interrupted
            animator.gameObject.GetComponent&lt;YourEventScriptWhichPublishesThisEvent&gt;().OnFightEnd(); 
        } 
    }
    </pre>
    * Attach this FightState script to the attack animation state in the Animator window.
    * Implement OnFightEnd() inside <em><strong>YourEventScriptWhichPublishesThisEvent</strong></em> script:
    <pre>
    public void OnFightEnd()
    {
        canAttack = true;
        // or
        MyUnityEvent?.Invoke();
    }
    </pre>
    * From there you can Invoke Unity event or whatever you like.
* Parallax
* 
