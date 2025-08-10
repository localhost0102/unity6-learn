using UnityEngine;
using Player.Commands;
using Player.Enums;

[System.Serializable]
public class PlayerCarrySettings
{
    [SerializeField] public Transform CarryPoint;
    
    [field: SerializeField] public float CarryRadius { get; set; } = 1f;
    [field: SerializeField] public float DropOffset { get; set; } = 0f;
    public GameObject CarriedObject { get; set; }
    public CarryStates CarryState { get; set; }

    public bool IsCarrying => CarryState == CarryStates.Carrying;

    public void ValidateNullable()
    {
        if (!CarryPoint)
            Debug.LogWarning("CarryPoint is not assigned.");
    }

    public void Setup(PlayerController playerController)
    {
        if (!CarryPoint)
            CarryPoint = FindObjects.FindChildWithTag(playerController.transform, nameof(CarryPoint).Trim());
    }
}
