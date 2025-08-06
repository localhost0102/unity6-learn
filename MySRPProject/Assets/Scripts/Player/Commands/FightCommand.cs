using Helpers;
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
        private bool _attackStarted;

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

            switch (_fightSettings.FightState)
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

            InvokeAttackSound();
            // Clear after 1 attack
            AnimationController.StartAttack();
        }
        
        // Called by PlayerController subscribed to Event for AnimationEvents on Slashing action
        public void SetSwordColliderAsTrigger<T>(T parameter)
        {
            if (parameter == null) return;
            
            bool animationStarted = TTypeConvert.ConvertToBool(parameter);
            _attackStarted = animationStarted;
            _swordCollider.isTrigger = !_attackStarted;
            
            if (!animationStarted)
                AnimationController.EndAttack();
        }
        
        private bool CanAttack()
        {
            return _playerSettings.IsGrounded && _playerSettings.Rb.linearVelocity.y <= Mathf.Epsilon && !_attackStarted;
        }

        private void InvokeAttackSound()
        {
            AttackEvent?.Invoke();
        }
    }
}