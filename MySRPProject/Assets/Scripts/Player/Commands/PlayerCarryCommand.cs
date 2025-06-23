using UnityEngine;

namespace Player.Commands
{
    public class PlayerCarryCommand : IPlayerCarryCommand
    {
        public CarryStates CarryState { get; set; }

        private readonly PlayerSettings _playerSettings;

        private readonly PlayerCarrySettings _playerCarrySettings;
        
        private GameObject _carriedObject;

        public PlayerCarryCommand(PlayerSettings playerSettings, PlayerCarrySettings playerCarrySettings)
        {
            _playerSettings = playerSettings;
            _playerCarrySettings = playerCarrySettings;
        }

        public void Execute()
        {
            switch (_playerCarrySettings.CarryState)
            {
                case CarryStates.Pickup:
                    Debug.Log("Pickup");
                    PickupObject(_playerCarrySettings.CarriedObject);
                    break;
                case CarryStates.Drop:
                    DropObject();
                    Debug.Log("Drop");
                    break;
                case CarryStates.Carrying:
                    Debug.Log("Carrying");
                    break;
                default: _playerCarrySettings.CarryState = CarryStates.None;
                    break;
            }
            
            if (_playerCarrySettings.CarryState != CarryStates.None)
                _playerCarrySettings.CarryState = CarryStates.None;
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
                rb.simulated = true;

            _carriedObject = null;
        }
    }
}