using Player;
using UnityEngine;

public class OnSwordCollision : MonoBehaviour
{
    [SerializeField] private GameObject _playerParent;
    
    private PlayerFightSettings _playerFightSettings;

    public void SetPlayerSettings(PlayerFightSettings playerFightSettings)
    {
        _playerFightSettings = playerFightSettings;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        
        rb.AddForce(Vector3.down * _playerFightSettings.SwordForce,  ForceMode2D.Impulse);
    }

    
}