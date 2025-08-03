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
    [SerializeField] public Transform GroundCheckBackRaycast;
    [SerializeField] public Transform GroundCheckMidRaycast;
    [SerializeField] public Transform GroundCheckFrontRaycast;
    [SerializeField] public Transform ForwardCheckRaycast;
    [SerializeField] public Transform ForwardCheckRaycastBottom;
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] public LayerMask BlockingLayer;
    [SerializeField] public Rigidbody2D Rb;
    [SerializeField] public Transform CarryPoint;

    // === Movement & Jumping ===
    [Header("Properties")]
    [SerializeField] public float MoveSpeed = 5f;
    [SerializeField] public int FacingDirection = 1; // 1 = right, -1 = left
    [SerializeField] public float JumpForce = 10f;
    [SerializeField] public float GroundCheckRadius = 0.5f;
    [SerializeField] public float GroundCheckRadiusRaycast = 0.3f;
    [SerializeField] public float ForwardCheckDistanceRaycast = 0.2f;
    [SerializeField] public float AirControlFactor = 0.016f;

    // === Runtime State ===
    [Header("Player States")]
    [SerializeField] public bool HasJumped;
    [SerializeField] public bool IsGrounded;

    public float LastGroundedDirection { get; set; }
    public Vector2 MoveInput { get; set; }
    public bool IsBlockedAhead { get; set; }

    public void ValidateNullable()
    {
        if (!GroundCheck)
            Debug.LogWarning("GroundCheck is not assigned.");
        
        if (!GroundCheckBackRaycast)
            Debug.LogWarning("GroundCheckBackRaycast is not assigned.");
        
        if (!GroundCheckMidRaycast)
            Debug.LogWarning("GroundCheckMidRaycast is not assigned.");
        
        if (!GroundCheckFrontRaycast)
            Debug.LogWarning("GroundCheckFrontRaycast is not assigned.");

        if (!ForwardCheckRaycast)
            Debug.LogWarning("ForwardCheckRaycast is not assigned.");
        
        if (!ForwardCheckRaycastBottom)
            Debug.LogWarning("ForwardCheckRaycastBottom is not assigned.");
        
        if (GroundLayer.value == 0)
            Debug.LogWarning("GroundLayer is not set.");
        
        if (BlockingLayer.value == 0)
            Debug.LogWarning("BlockingLayer (walls, etc.) is not set.");

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

        if (!GroundCheckBackRaycast)
            GroundCheckBackRaycast = FindObjects.FindChildByName(playerController.transform, nameof(GroundCheckBackRaycast));

        if (!GroundCheckMidRaycast)
            GroundCheckMidRaycast = FindObjects.FindChildByName(playerController.transform, nameof(GroundCheckMidRaycast));

        if (!GroundCheckFrontRaycast)
            GroundCheckFrontRaycast = FindObjects.FindChildByName(playerController.transform, nameof(GroundCheckFrontRaycast));
        
        if (!ForwardCheckRaycast)
            ForwardCheckRaycast = FindObjects.FindChildByName(playerController.transform, nameof(ForwardCheckRaycast));

        if (!ForwardCheckRaycastBottom)
            ForwardCheckRaycastBottom = FindObjects.FindChildByName(playerController.transform, nameof(ForwardCheckRaycastBottom));

        
        if (!Animator)
        {
            Animator = playerController.GetComponent<Animator>();
            AnimationController.Animator = Animator;
        }
    }
}
