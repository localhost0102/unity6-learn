using Player.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Commands
{
    public class FightCommand : IPlayerCommand
    {
        public static readonly UnityEvent AttackEvent = new UnityEvent();
        
        private readonly PlayerSettings _playerSettings;
        private readonly PlayerFightSettings _fightSettings;
        
        public FightCommand(PlayerSettings playerSettings, PlayerFightSettings fightSettings)
        {
            _playerSettings = playerSettings;
            _fightSettings = fightSettings;
        }

        public void Execute()
        {
            if (_fightSettings.FightState == FightStates.None) return;

            switch (_fightSettings.FightState )
            {
                case FightStates.Slash:
                    ExecuteSlash();
                    break;
            }
        }

        private void ExecuteSlash()
        {
            _fightSettings.FightState = FightStates.None;
            if (CanAttack() == false) return;
            
            InvokeAttackEvent();
            // Clear after 1 attack
            AnimationController.SetSlash();
        }

        private bool CanAttack()
        {
            return _playerSettings.IsGrounded && _playerSettings.Rb.linearVelocity.y <= Mathf.Epsilon;
        }

        private void InvokeAttackEvent()
        {
            AttackEvent?.Invoke();
        }
    }
}