using UnityEngine;

[System.Serializable]
public class PlayerSettings
{
    // Here are used public fields instead of get/set because Header attribute won't work on properties.
    [Header("Objects")]
    [SerializeField] public Transform GroundCheck;
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] public Rigidbody2D Rb;
    
    [Header("Properties")]
    [SerializeField] public float MoveSpeed = 5f;
    [SerializeField] public float JumpForce = 10f;
    [SerializeField] public float GroundCheckRadius = 0.5f;
    [SerializeField] public float AirControlFactor = 0.016f;
    
    [Header("Player states")]
    [SerializeField] public bool IsJumping;
    [SerializeField] public bool IsGrounded;
    
    public float LastGroundedDirection { get; set; } = 0f;
    public Vector2 MoveInput { get; set; }

    public void ValidateNullable()
    {
        if (!GroundCheck)
            Debug.LogWarning("Ground check is missing");
        
        if (GroundLayer.value == 0)
            Debug.LogWarning("Ground layer is not selected");

        if (!Rb)
            Debug.LogWarning("RigidBody is null");
    }

    public void Setup(PlayerController playerController)
    {
        Rb = playerController.GetComponent<Rigidbody2D>();
        GroundCheck = FindObjects.FindChildWithTag(playerController.transform, nameof(GroundCheck));
    }
}
