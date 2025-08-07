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
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        _playerSoundController.PlayAttackHitSound();
        rb.AddForce(Vector3.down * _playerFightSettings.SwordForce,  ForceMode2D.Impulse);
    }

    
}