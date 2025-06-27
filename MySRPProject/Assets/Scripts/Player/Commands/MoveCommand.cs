using System.Windows.Input;
using UnityEngine;

namespace Player.Commands
{
    public class MoveCommand : IPlayerCommand
    {
        private readonly PlayerSettings _playerSettings;

        public MoveCommand(PlayerSettings playerSettings)
        {
            _playerSettings =  playerSettings;
        }
        
        public void Execute()
        {
            Vector2 velocity = _playerSettings.Rb.linearVelocity;
            
            if (_playerSettings.IsGrounded)
            {
                velocity = GroundMovement(velocity);
                AnimationController.SetWalking(velocity.x);
            }
            else
            {
                velocity = AirMovement(velocity);
            }

            // Apply final velocity
            _playerSettings.Rb.linearVelocity = velocity;
        }

        private Vector2 GroundMovement(Vector2 velocity)
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

            return velocity;
        }

        private Vector2 AirMovement(Vector2 velocity)
        {
            // Limited air control
            velocity.x = Mathf.Clamp(velocity.x, -_playerSettings.MoveSpeed, _playerSettings.MoveSpeed);

            // Slight influence if player is still holding a direction
            if (Mathf.Abs(_playerSettings.MoveInput.x) > 0.1f)
                velocity.x += _playerSettings.MoveInput.x * _playerSettings.MoveSpeed * _playerSettings.AirControlFactor;
            return velocity;
        }
    }
    
}