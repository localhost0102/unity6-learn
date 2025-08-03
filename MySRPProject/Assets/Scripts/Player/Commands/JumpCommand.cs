using UnityEngine;

namespace Player.Commands
{
    public class JumpCommand : IPlayerCommand
    {
        private readonly PlayerSettings _playerSettings;
        private float _previousVelocityY = 0f;
        
        public JumpCommand(PlayerSettings playerSettings)
        {
            _playerSettings = playerSettings;
        }
        
        public void Execute()
        {
            SetFalling(_playerSettings.Rb.linearVelocity.y);
            
            if (_playerSettings.IsGrounded && _playerSettings.Rb.linearVelocity.y <= 0f)
            {
                AnimationController.SetJumping(false);
            }
            
            if (_playerSettings.HasJumped)
            {
                AnimationController.SetJumping(true);
                Vector2 velocity = _playerSettings.Rb.linearVelocity;
                velocity.y = _playerSettings.JumpForce;
                _playerSettings.Rb.linearVelocity = velocity;
                _playerSettings.HasJumped = false;
            }

            SetPreviousVelocity(_playerSettings.Rb.linearVelocity.y);
        }

        private void SetPreviousVelocity(float linearVelocityY)
        {
            _previousVelocityY = linearVelocityY;
        }

        private void SetFalling(float currentVelocityY)
        {
            //currentVelocityY < previousVelocityY && _previousVelocityY >= 0
            float roundedVelocityY = Mathf.Round(currentVelocityY * 100) / 100f;
            if (roundedVelocityY < -0.1f)
            {
                AnimationController.SetFalling(true);
            }
            else
            {
                AnimationController.SetFalling(false);
            }
        }
    }
}