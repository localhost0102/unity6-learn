using System.Collections.Generic;
using Helpers;
using Player.Commands;
using Player.Enums;
using UnityEngine;

// ...your existing usings and namespace...

public class CarryCommand : IPlayerCarryCommand
{
    public CarryStates CarryState { get; set; }

    private readonly PlayerSettings _playerSettings;
    private readonly PlayerCarrySettings _playerCarrySettings;

    private GameObject _carriedObject;
    private Transform _originalParent;

    public CarryCommand(PlayerSettings playerSettings, PlayerCarrySettings playerCarrySettings)
    {
        _playerSettings = playerSettings;
        _playerCarrySettings = playerCarrySettings;
    }

    public void Execute()
    {
        switch (_playerCarrySettings.CarryState)
        {
            case CarryStates.Pickup:
                PickupObject(_playerCarrySettings.CarriedObject);
                break;
            case CarryStates.Drop:
                DropObject();
                break;
            case CarryStates.Carrying:
                break;
            default:
                _playerCarrySettings.CarryState = CarryStates.None;
                break;
        }
    }

    public GameObject FindNearbyCarriable()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(_playerCarrySettings.CarryPoint.position, _playerCarrySettings.CarryRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<CarriableObject>(out var _))
                return hit.gameObject;
        }

        return null;
    }

    private void PickupObject(GameObject objToPickup)
    {
        if (_carriedObject || !objToPickup || !_playerSettings.CarryPoint) return;

        _carriedObject = objToPickup;
        _originalParent = objToPickup.transform.parent;
        
        // Attach to carry point while preserving world position, rotation, and scale
        _carriedObject.transform.SetParent(_playerSettings.CarryPoint, worldPositionStays: true);

        // Disable physics while carrying
        var rb = _carriedObject.GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;
        
        _playerCarrySettings.CarryState = CarryStates.Carrying;
        _playerSettings.IsCarryingObject = true;
    }

    private void DropObject()
    {
        if (_carriedObject == null) return;

        _playerSettings.IsCarryingObject = false;

        // 1) Decide where in WORLD space you want to drop it.
        Vector3 dropWorldPos = _carriedObject.transform.position;

        // Optional: push a bit in front of player (world space)
        dropWorldPos += (Vector3)_playerSettings.CarryPoint.transform.right * _playerSettings.FacingDirection *
                       _playerCarrySettings.DropOffset;

        // 2) Reparent to the original parent while KEEPING world TRS (prevents any X/Y/Z pop)
        _carriedObject.transform.SetParent(_originalParent, worldPositionStays: true);

        // 3) Re-apply the world position you want (safe; still world space)
        _carriedObject.transform.position = dropWorldPos;

        // 4) Re-enable physics and give it some inherited velocity
        var rb = _carriedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = new Vector2(
                _playerSettings.Rb.linearVelocity.x * 1.3f,
                _playerSettings.Rb.linearVelocity.y * 1.3f
            );
        }

        _carriedObject = null;
        _playerCarrySettings.CarryState = CarryStates.None;
    }

    // Debug only
    public void OnDrawGizmosSelected()
    {
        if (_playerCarrySettings == null || _playerCarrySettings.CarryPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_playerCarrySettings.CarryPoint.position, _playerCarrySettings.CarryRadius);
    }
}