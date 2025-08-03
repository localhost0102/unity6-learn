using Player.Commands;
using Unity.Properties;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    // AudioSource je stavljen na child u slucaju da zelimo vise sourceva. Drugi bismo dodali na novi child objekt
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip footstepSound1;
    [SerializeField] private AudioClip footstepSound2;
    [SerializeField] private AudioClip jumpSound1;
    [SerializeField] private AudioClip jumpSound2;
    [SerializeField] private AudioClip jumpSound3;
    [SerializeField] private AudioClip landingSound1;
    [SerializeField] private AudioClip landingSound2;
    [SerializeField] private AudioClip landingSound3;
    [SerializeField] private AudioClip lightSaberSound1;

    void OnEnable()
    {
        PlayerAnimationEvents.FootstepEvent.AddListener(PlayFootstepSound);
        JumpCommand.JumpEvent.AddListener(PlayJumpSound);
        JumpCommand.LandingEvent.AddListener(PlayLandedSound);
        FightCommand.AttackEvent.AddListener(PlayAttackSound);
    }

    void OnDisable()
    {
        PlayerAnimationEvents.FootstepEvent.RemoveListener(PlayFootstepSound);
        JumpCommand.JumpEvent.RemoveListener(PlayJumpSound);
        JumpCommand.LandingEvent.RemoveListener(PlayLandedSound);
        FightCommand.AttackEvent.RemoveListener(PlayAttackSound);
    }

    private void PlayFootstepSound(string soundOrdinal)
    {
        if (soundOrdinal == "1")
            sfxSource.PlayOneShot(footstepSound1);
        else
            sfxSource.PlayOneShot(footstepSound2);
    }

    private void PlayJumpSound()
    {
        int randomInt = Random.Range(1, 3); // 1 Inclusive, Max exclusive

        switch (randomInt)
        {
            case 1: sfxSource.PlayOneShot(jumpSound1); break;
            case 2: sfxSource.PlayOneShot(jumpSound2); break;
        }
    }

    private void PlayLandedSound()
    {
        // Use stop because landing sometimes causes multiple sounds to trigger in short time (whenever it hits ground in JumpCommand)
        // Still not at the best, consider not playing if one is already active
        int randomInt = Random.Range(1, 4);
        sfxSource.Stop();
        
        switch (randomInt)
        {
            case 1: sfxSource.clip = landingSound1; break;
            case 2: sfxSource.clip = landingSound2; break;
            case 3: sfxSource.clip = landingSound3; break;
        }

        sfxSource.Play();
    }

    private void PlayAttackSound()
    {
        sfxSource.PlayOneShot(lightSaberSound1);
    }
}