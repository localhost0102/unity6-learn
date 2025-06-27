namespace Player.Commands
{
    public class PlayerCommandFactory
    {
        private readonly PlayerSettings _playerSettings;
        private readonly PlayerCarrySettings _playerCarrySettings;
        private readonly PlayerFightSettings _playerFightSettings;

        public PlayerCommandFactory(PlayerSettings playerSettings, PlayerCarrySettings playerCarrySettings, PlayerFightSettings playerFightSettings)
        {
            _playerSettings =  playerSettings;
            _playerCarrySettings = playerCarrySettings;
            _playerFightSettings = playerFightSettings;
        }
        
        public IPlayerCommand CreatePlayerMoveCommand()
        {
            return new MoveCommand(_playerSettings);
        }
        
        public IPlayerCommand CreatePlayerJumpCommand()
        {
            return new JumpCommand(_playerSettings);
        }
        
        public IPlayerCarryCommand CreatePlayerCarryCommand()
        {
            return new CarryCommand(_playerSettings, _playerCarrySettings);
        }
        
        public IPlayerCommand CreatePlayerFightCommand()
        {
            return new FightCommand(_playerSettings, _playerFightSettings);
        }
    }
}