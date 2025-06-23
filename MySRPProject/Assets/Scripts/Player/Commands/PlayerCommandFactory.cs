namespace Player.Commands
{
    public class PlayerCommandFactory
    {
        private readonly PlayerSettings _playerSettings;
        private readonly PlayerCarrySettings _playerCarrySettings;

        public PlayerCommandFactory(PlayerSettings playerSettings, PlayerCarrySettings playerCarrySettings)
        {
            _playerSettings =  playerSettings;
            _playerCarrySettings = playerCarrySettings;
        }
        
        public IPlayerCommand CreatePlayerMoveCommand()
        {
            return new PlayerMoveCommand(_playerSettings);
        }
        
        public IPlayerCommand CreatePlayerJumpCommand()
        {
            return new PlayerJumpCommand(_playerSettings);
        }
        
        public IPlayerCarryCommand CreatePlayerCarryCommand()
        {
            return new PlayerCarryCommand(_playerSettings, _playerCarrySettings);
        }
    }
}