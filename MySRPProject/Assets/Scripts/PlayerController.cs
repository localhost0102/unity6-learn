using Player;
using Player.Commands;
using Player.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private PlayerCarrySettings _playerCarrySettings;
    [SerializeField] private PlayerFightSettings _playerFightSettings;
    [SerializeField] private bool _debug;
    
    private PlayerControls _controls;
    private IPlayerCommand _moveCommand;
    private IPlayerCommand _jumpCommand;
    private IPlayerWithEventsCommand _fightCommand;
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
        SetupChildren();
    }

    private void SetupChildren()
    {
        Transform swordObject = FindObjects.FindChildByName(transform,"Sword");
        OnSwordCollision swordCollision = swordObject.GetComponent<OnSwordCollision>();
        swordCollision.SetPlayerSettings(_playerFightSettings);
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
        
        // TODO: Ovo trenutno salje samo FightEnd, a meni treba i Start u PlayerAnimationEvent (moram prepoznati kada mac mase, a kada udari u EnableSwordCollider
        PlayerAnimationEvents.SlashEvent.AddListener(_fightCommand.EnableSwordCollider);
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
        PlayerAnimationEvents.SlashEvent.RemoveListener(_fightCommand.EnableSwordCollider);
    }

    private void FixedUpdate()
    {
        _playerSettings.IsGrounded = IsGroundedRaycast();
        _playerSettings.IsBlockedAhead =
            IsBlockedAhead(new Vector2(_playerSettings.FacingDirection, 0), _playerSettings.ForwardCheckRaycastBottom.position) ||
            IsBlockedAhead(new Vector2(_playerSettings.FacingDirection, 0), _playerSettings.ForwardCheckRaycast.position);

        //SetIsGroundedOldWay();

        _jumpCommand.Execute();
        _fightCommand.Execute();
        _moveCommand.Execute();
        CarryObject();
    }

    private bool IsGroundedRaycast()
    {
        return
            CheckIsGroundedRaycast(_playerSettings.GroundCheckFrontRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer) ||
            CheckIsGroundedRaycast(_playerSettings.GroundCheckBackRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer) ||
            CheckIsGroundedRaycast(_playerSettings.GroundCheckMidRaycast.position, _playerSettings.GroundCheckRadiusRaycast, _playerSettings.GroundLayer);
    }

    private bool CheckIsGroundedRaycast(Vector3 groundcheckRaycastPosition, float lengthToCheck, LayerMask layersToCheck)
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundcheckRaycastPosition, Vector2.down, lengthToCheck, layersToCheck);
        bool hasHit = raycastHit.collider != null;

        if (_debug)
            Debug.DrawRay(groundcheckRaycastPosition, Vector2.down * lengthToCheck, hasHit ? Color.green : Color.red);

        if (hasHit)
            return true;

        return false;
    }

    private bool IsBlockedAhead(Vector2 direction, Vector2 origin)
    {
        //Vector2 origin = _playerSettings.ForwardCheckRaycast.position;
        float distance = _playerSettings.ForwardCheckDistanceRaycast;
        int layer = _playerSettings.BlockingLayer;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layer);

        // Force cast to Vector3 for draw
        if (_debug)
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