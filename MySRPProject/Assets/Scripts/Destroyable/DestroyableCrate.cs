using System;
using Destroyable;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyableCrate : MonoBehaviour, IDestroyable
{
    [SerializeField] private byte hitsToDestroy = 4;
    private static int _numberOfHits = 0;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("WeaponTag"))
        {
            _numberOfHits++;
            if (_numberOfHits >= hitsToDestroy) DestroyTheItem();
        }
    }

    public void DestroyTheItem()
    {
        gameObject.SetActive(false);
    }
}