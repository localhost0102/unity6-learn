using Player.Enums;
using UnityEngine;

namespace Player.Commands
{
    public class CarryCommand : IPlayerCarryCommand
    {
        public CarryStates CarryState { get; set; }

        private readonly PlayerSettings _playerSettings;

        private readonly PlayerCarrySettings _playerCarrySettings;

        private GameObject _carriedObject;

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

        private void PickupObject(GameObject objToPickup)
        {
            if (_carriedObject || !objToPickup || !_playerSettings.CarryPoint) return;

            _carriedObject = objToPickup;
            _carriedObject.transform.SetParent(_playerSettings.CarryPoint);
            _carriedObject.transform.localPosition = Vector3.zero;
            _playerCarrySettings.CarryState = CarryStates.Carrying;

            var rb = _carriedObject.GetComponent<Rigidbody2D>();
            if (rb)
                rb.simulated = false;
        }

        private void DropObject()
        {
            if (_carriedObject == null) return;

            _carriedObject.transform.SetParent(null);

            var rb = _carriedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.linearVelocity = Vector2.zero;
            }

            // Drop slightly in front of the player
            //_carriedObject.transform.position += Vector3.right * _playerSettings.FacingDirection;
            rb.linearVelocity = new Vector2(_playerSettings.Rb.linearVelocity.x * 1.3f, _playerSettings.Rb.linearVelocity.y * 1.3f);
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
}