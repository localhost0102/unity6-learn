using Player;
using UnityEngine;
using Player.Commands;

[System.Serializable]
public class PlayerSettings
{
    // === References ===
    [Header("Objects")] 
    [SerializeField] private Animator Animator;
    [SerializeField] public Transform GroundCheck;
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] public Rigidbody2D Rb;
    [SerializeField] public Transform CarryPoint;

    // === Movement & Jumping ===
    [Header("Properties")]
    [SerializeField] public float MoveSpeed = 5f;
    [SerializeField] public int FacingDirection = 1; // 1 = right, -1 = left
    [SerializeField] public float JumpForce = 10f;
    [SerializeField] public float GroundCheckRadius = 0.5f;
    [SerializeField] public float AirControlFactor = 0.016f;

    // === Runtime State ===
    [Header("Player States")]
    [SerializeField] public bool HasJumped;
    [SerializeField] public bool IsGrounded;

    public float LastGroundedDirection { get; set; }
    public Vector2 MoveInput { get; set; }

    public void ValidateNullable()
    {
        if (!GroundCheck)
            Debug.LogWarning("GroundCheck is not assigned.");

        if (GroundLayer.value == 0)
            Debug.LogWarning("GroundLayer is not set.");

        if (!Rb)
            Debug.LogWarning("Rigidbody2D is not assigned.");

        if (!CarryPoint)
            Debug.LogWarning("CarryPoint is not assigned.");
        
        if (!Animator)
            Debug.LogWarning("Animator is not assigned.");
    }

    public void Setup(PlayerController playerController)
    {
        Rb = playerController.GetComponent<Rigidbody2D>();

        if (!GroundCheck)
            GroundCheck = FindObjects.FindChildWithTag(playerController.transform, nameof(GroundCheck));
        
        if (!CarryPoint)
            CarryPoint = FindObjects.FindChildWithTag(playerController.transform, nameof(CarryPoint).Trim());

        if (!Animator)
        {
            Animator = playerController.GetComponent<Animator>();
            AnimationController.Animator = Animator;
        }
    }
}
