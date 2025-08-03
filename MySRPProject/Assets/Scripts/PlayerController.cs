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

        PlayerCommandFactory factory = new PlayerCommandFactory(_playerSettings, _playerCarrySettings, _playerFightSettings);
        _moveCommand = factory.CreatePlayerMoveCommand();
        _jumpCommand = factory.CreatePlayerJumpCommand();
        _fightCommand = factory.CreatePlayerFightCommand();
        _carryCommand = factory.CreatePlayerCarryCommand();

        SetupInputControls();
    }

    private void SetupInputControls()
    {
        _controls = new PlayerControls();
        // Move
        _controls.Gameplay.Move.performed += ctx => SetMoveDirection(ctx.ReadValue<Vector2>());
        _controls.Gameplay.Move.canceled += _ => _playerSettings.MoveInput = Vector2.zero;
        // Jump
        _controls.Gameplay.Jump.performed += _ =>
        {
            //if (SetIsGroundedOldWay()) _playerSettings.HasJumped = true;
            if (IsGroundedRaycast()) _playerSettings.HasJumped = true;
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
        _playerSettings.IsGrounded = IsGroundedRaycast();
        _playerSettings.IsBlockedAhead =
            IsBlockedAhead(new Vector2(_playerSettings.FacingDirection, 0), _playerSettings.ForwardCheckRaycastBottom.position) ||
            IsBlockedAhead(new Vector2(_playerSettings.FacingDirection, 0), _playerSettings.ForwardCheckRaycast.position);


        //SetIsGroundedOldWay();

        _jumpCommand.Execute();
        _moveCommand.Execute();
        _fightCommand.Execute();
        CarryObject();
    }

    private bool IsGroundedRaycast()
    {
        return
            CheckIsGroundedRaycast(_playerSettings.GroundCheckFrontRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer, debug: true) ||
            CheckIsGroundedRaycast(_playerSettings.GroundCheckBackRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer, debug: true) ||
            CheckIsGroundedRaycast(_playerSettings.GroundCheckMidRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer, debug: true);
    }

    private bool CheckIsGroundedRaycast(Vector3 groundcheckRaycastPosition, float lengthToCheck, LayerMask layersToCheck, bool debug = false)
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundcheckRaycastPosition, Vector2.down, lengthToCheck, layersToCheck);
        bool hasHit = raycastHit.collider != null;

        if (debug)
            Debug.DrawRay(groundcheckRaycastPosition, Vector2.down * lengthToCheck, hasHit ? Color.green : Color.red);

        if (hasHit)
        {
            // _playerSettings.IsGrounded = true;
            // return _playerSettings.IsGrounded;
            return true;
        }

        return false;
    }

    private bool IsBlockedAhead(Vector2 direction, Vector2 origin)
    {
        //Vector2 origin = _playerSettings.ForwardCheckRaycast.position;
        float distance = _playerSettings.ForwardCheckDistanceRaycast;
        int layer = _playerSettings.BlockingLayer;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layer);

        // Force cast to Vector3 for draw
        Debug.DrawRay(origin, (Vector3)(direction * distance), hit.collider != null ? Color.green : Color.red);

        return hit.collider != null && !IsMovable(hit.collider.gameObject);
    }

    private bool IsMovable(GameObject obj)
    {
        //return obj.CompareTag("Movable");
        return false;
    }

    private void CarryObject()
    {
        if (_playerCarrySettings.CarryState == CarryStates.Pickup)
        {
            _playerCarrySettings.CarriedObject = _carryCommand.FindNearbyCarriable();
        }

        _carryCommand.Execute();
    }

    private void SetMoveDirection(Vector2 readValue)
    {
        _playerSettings.MoveInput = readValue;

        if (_playerSettings.MoveInput.x != 0)
        {
            _playerSettings.FacingDirection = _playerSettings.MoveInput.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * _playerSettings.FacingDirection,
                transform.localScale.y, transform.localScale.z);
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

    private bool SetIsGroundedOldWay()
    {
        _playerSettings.IsGrounded = Physics2D.OverlapCircle(_playerSettings.GroundCheck.position, _playerSettings.GroundCheckRadius, _playerSettings.GroundLayer);
        return _playerSettings.IsGrounded;
    }
}