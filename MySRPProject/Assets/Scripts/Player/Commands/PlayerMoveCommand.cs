using System.Windows.Input;
using UnityEngine;

namespace Player
{
    public class PlayerMoveCommand : IPlayerCommand
    {
        private readonly PlayerSettings _playerSettings;

        public PlayerMoveCommand(PlayerSettings playerSettings)
        {
            _playerSettings =  playerSettings;
        }
        
        public void Execute()
        {
            Vector2 velocity = _playerSettings.Rb.linearVelocity;
            
            if (_playerSettings.IsGrounded)
            {
                // Full control
                velocity.x = _playerSettings.MoveInput.x * _playerSettings.MoveSpeed;

                // Remember last move input direction while grounded
                if (Mathf.Abs(_playerSettings.MoveInput.x) > 0.1f)
                {
                    _playerSettings.LastGroundedDirection = _playerSettings.MoveInput.x;
                }
                else
                {
                    // Reset momentum direction when grounded and idle
                    _playerSettings.LastGroundedDirection = 0f;
                }

            }
            else
            {
                // Limited air control
            
                velocity.x = Mathf.Clamp(velocity.x, -_playerSettings.MoveSpeed, _playerSettings.MoveSpeed);

                // Slight influence if player is still holding a direction
                if (Mathf.Abs(_playerSettings.MoveInput.x) > 0.1f)
                    velocity.x += _playerSettings.MoveInput.x * _playerSettings.MoveSpeed * _playerSettings.AirControlFactor;
            }

            // Apply final velocity
            _playerSettings.Rb.linearVelocity = velocity;
        }
    }
    
}