using Player.Commands;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private PlayerCarrySettings _playerCarrySettings;

    private PlayerControls _controls;
    private IPlayerCommand _moveCommand;
    private IPlayerCommand _jumpCommand;
    private IPlayerCarryCommand _carryCommand;
    private const string GroundLayerName = "Ground";

    private void Awake()
    {
        playerSettings.Setup(this);
        playerSettings.ValidateNullable();

        _playerCarrySettings.Setup(this);
        _playerCarrySettings.ValidateNullable();

        PlayerCommandFactory factory = new PlayerCommandFactory(playerSettings, _playerCarrySettings);
        _moveCommand = factory.CreatePlayerMoveCommand();
        _jumpCommand = factory.CreatePlayerJumpCommand();
        _carryCommand = factory.CreatePlayerCarryCommand();

        _controls = new PlayerControls();
        _controls.Gameplay.Move.performed += ctx => playerSettings.MoveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += _ => playerSettings.MoveInput = Vector2.zero;
        _controls.Gameplay.Jump.performed += _ =>
        {
            if (IsGrounded()) playerSettings.IsJumping = true;
        };

        _controls.Gameplay.CarryObject.performed += _ => _playerCarrySettings.CarryState = CarryStates.Pickup;
        _controls.Gameplay.CarryObject.canceled += _ => _playerCarrySettings.CarryState = CarryStates.Drop;
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
        playerSettings.IsGrounded = IsGrounded();
        _moveCommand.Execute();
        _jumpCommand.Execute();
        CarryObject();
        FindNearbyCarriable();
    }

    public GameObject FindNearbyCarriable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_playerCarrySettings.CarryPoint.position, _playerCarrySettings.CarryRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<CarriableObject>(out var carriable))
            {
                return hit.gameObject;
            }
        }
        return null;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (_playerCarrySettings == null || _playerCarrySettings.CarryPoint == null) return;
Debug.Log("Gizmos");
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_playerCarrySettings.CarryPoint.position, _playerCarrySettings.CarryRadius);
    }
    
    private void CarryObject()
    {
        if (_playerCarrySettings.CarryState == CarryStates.Pickup)
        {
            _playerCarrySettings.CarriedObject = _carryCommand.FindNearbyCarriable();
        }

        _carryCommand.Execute();
    }

    // Used only to initially set IsGrounded property (for testing purposes)
    private void OnCollisionEnter2D(Collision2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (layerName == GroundLayerName)
            playerSettings.IsGrounded = true;
    }

    private bool IsGrounded()
    {
        playerSettings.IsGrounded = Physics2D.OverlapCircle(playerSettings.GroundCheck.position, playerSettings.GroundCheckRadius, playerSettings.GroundLayer);
        return playerSettings.IsGrounded;
    }
}