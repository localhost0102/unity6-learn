using System;
using System.Windows.Input;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings playerSettings;

    private PlayerControls controls;
    private IPlayerCommand _moveCommand;
    private IPlayerCommand _jumpCommand;
    private const string GroundLayerName = "Ground";

    private void Awake()
    {
        playerSettings.Setup(this);
        playerSettings.ValidateNullable();
        PlayerCommandFactory factory = new PlayerCommandFactory(playerSettings);
        _moveCommand = factory.CreatePlayerMoveCommand();
        _jumpCommand = factory.CreatePlayerJumpCommand();

        controls = new PlayerControls();
        controls.Gameplay.Move.performed += ctx => playerSettings.MoveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => playerSettings.MoveInput = Vector2.zero;
        controls.Gameplay.Jump.performed += ctx =>
        {
            if (IsGrounded()) playerSettings.IsJumping = true;
        };
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void FixedUpdate()
    {
        playerSettings.IsGrounded = IsGrounded();
        _moveCommand.Execute();
        _jumpCommand.Execute();
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