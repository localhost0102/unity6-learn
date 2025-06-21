namespace Player
{
    public class PlayerCommandFactory
    {
        private readonly PlayerSettings _playerSettings;

        public PlayerCommandFactory(PlayerSettings playerSettings)
        {
            _playerSettings =  playerSettings;
        }
        
        public IPlayerCommand CreatePlayerMoveCommand()
        {
            return new PlayerMoveCommand(_playerSettings);
        }
        
        public IPlayerCommand CreatePlayerJumpCommand()
        {
            return new PlayerJumpCommand(_playerSettings);
        }
    }
}