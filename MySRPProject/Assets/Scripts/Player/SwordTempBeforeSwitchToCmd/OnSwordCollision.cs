using Player;
using UnityEngine;

public class OnSwordCollision : MonoBehaviour
{
    [SerializeField] private GameObject _playerParent;
    [SerializeField] private PlayerSoundController _playerSoundController;
    
    private PlayerFightSettings _playerFightSettings;

    public void SetPlayerSettings(PlayerFightSettings playerFightSettings)
    {
        _playerFightSettings = playerFightSettings;
        
        if (_playerSoundController == null)
            Debug.Log($"{nameof(_playerFightSettings)} is null in OnSwordCollision script. Please attach it from the Player");
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
   
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        _playerSoundController.PlayAttackHitSound();
        rb.AddForce(Vector3.down * _playerFightSettings.SwordForce,  ForceMode2D.Impulse);
    }

    
}