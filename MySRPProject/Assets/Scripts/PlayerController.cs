using Player;
using Player.Commands;
using Player.Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private PlayerCarrySettings _playerCarrySettings;
    [SerializeField] private PlayerFightSettings _playerFightSettings;

    private PlayerControls _controls;
    private IPlayerCommand _moveCommand;
    private IPlayerCommand _jumpCommand;
    private IPlayerCommand _fightCommand;
    private IPlayerCarryCommand _carryCommand;
    
    private const string GroundLayerName = "Ground";

    private void Awake()
    {
        _playerSettings.Setup(this);
        _playerSettings.ValidateNullable();
        _playerCarrySettings.Setup(this);
        _playerCarrySettings.ValidateNullable();

        PlayerCommandFactory factory = new PlayerCommandFactory(_playerSettings, _playerCarrySettings,  _playerFightSettings);
        _moveCommand = factory.CreatePlayerMoveCommand();
        _jumpCommand = factory.CreatePlayerJumpCommand();
        _fightCommand =  factory.CreatePlayerFightCommand();
        _carryCommand = factory.CreatePlayerCarryCommand();
        
        SetupInputControls();
    }

    private void SetupInputControls()
    {
        _controls = new PlayerControls();
        // Move
        _controls.Gameplay.Move.performed += ctx => SetDirection(ctx.ReadValue<Vector2>());
        _controls.Gameplay.Move.canceled += _ => _playerSettings.MoveInput = Vector2.zero;
        // Jump
        _controls.Gameplay.Jump.performed += _ =>
        {
            if (IsGrounded()) _playerSettings.HasJumped = true;
        };

        // Carry
        _controls.Gameplay.CarryObject.performed += _ => _playerCarrySettings.CarryState = CarryStates.Pickup;
        _controls.Gameplay.CarryObject.canceled += _ => _playerCarrySettings.CarryState = CarryStates.Drop;
        // Fight
        _controls.Gameplay.Slash.performed += _ => _playerFightSettings.FightState = FightStates.Slash;
        _controls.Gameplay.Slash.canceled += _ => _playerFightSettings.FightState = FightStates.None;
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void FixedUpdate()
    {
        _playerSettings.IsGrounded = IsGrounded();
        _moveCommand.Execute();
        _jumpCommand.Execute();
        _fightCommand.Execute();
        CarryObject();
    }
    
    private void CarryObject()
    {
        if (_playerCarrySettings.CarryState == CarryStates.Pickup)
        {
            _playerCarrySettings.CarriedObject = _carryCommand.FindNearbyCarriable();
        }

        _carryCommand.Execute();
    }
    
    private void SetDirection(Vector2 readValue)
    {
        _playerSettings.MoveInput = readValue;
        
        if (_playerSettings.MoveInput.x != 0)
        {
            _playerSettings.FacingDirection = _playerSettings.MoveInput.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _playerSettings.FacingDirection, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //_carryCommand.OnDrawGizmosSelected();
    }
    
    // Used only to initially set IsGrounded property (for testing purposes)
    private void OnCollisionEnter2D(Collision2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == GroundLayerName)
            _playerSettings.IsGrounded = true;
    }

    private bool IsGrounded()
    {
        _playerSettings.IsGrounded = Physics2D.OverlapCircle(_playerSettings.GroundCheck.position, _playerSettings.GroundCheckRadius, _playerSettings.GroundLayer);
        return _playerSettings.IsGrounded;
    }
}