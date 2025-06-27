using Player.Enums;
using UnityEngine;

namespace Player.Commands
{
    public class FightCommand : IPlayerCommand
    {
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

            switch (_fightSettings.FightState)
            {
                case FightStates.Slash:
                    ExecuteSlash();
                    break;
            }
        }

        private void ExecuteSlash()
        {
            // Clear after 1 attack
            _fightSettings.FightState = FightStates.None;
            AnimationController.SetSlash();
        }
    }
}