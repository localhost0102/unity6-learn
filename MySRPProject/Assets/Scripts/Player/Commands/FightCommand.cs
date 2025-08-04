using Player.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Commands
{
    public class FightCommand : IPlayerWithEventsCommand
    {
        public static readonly UnityEvent AttackEvent = new UnityEvent();

        private readonly PlayerSettings _playerSettings;
        private readonly PlayerFightSettings _fightSettings;
        private readonly Collider2D _swordCollider;

        public FightCommand(PlayerSettings playerSettings, PlayerFightSettings fightSettings)
        {
            _playerSettings = playerSettings;
            _fightSettings = fightSettings;
            
            FindObjects.FindChildByName(_playerSettings.Rb.transform, "Sword");
            _swordCollider = _fightSettings.Sword?.GetComponent<Collider2D>();
        }

        public void Execute()
        {
            if (_fightSettings.FightState == FightStates.None) return;
            //_swordCollider.isTrigger = true;
            
            switch (_fightSettings.FightState)
            {
                case FightStates.Slash:
                    ExecuteSlash();
                    break;
            }
        }

        public void ActionEvent<T>(T parameter)
        {
            Debug.Log(parameter);
        }

        private void ExecuteSlash()
        {
            _fightSettings.FightState = FightStates.None;
            if (CanAttack() == false) return;

            InvokeAttackEvent();
            // Clear after 1 attack
            AnimationController.SetSlash();
            _swordCollider.isTrigger = false;
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