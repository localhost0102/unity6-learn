using UnityEngine;

namespace Player
{
    public class PlayerJumpCommand : IPlayerCommand
    {
        private readonly PlayerSettings _playerSettings;

        public PlayerJumpCommand(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }
        
        public void Execute()
        {
            if (_playerSettings.IsJumping)
            {
                Vector2 velocity = _playerSettings.Rb.linearVelocity;
                velocity.y = _playerSettings.JumpForce;
                _playerSettings.Rb.linearVelocity = velocity;
                _playerSettings.IsJumping = false;
            }
        }
    }
}