using System;
using Player.Enums;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerFightSettings
    {
        [field: SerializeField] public FightStates FightState { get; set; }
        [field: SerializeField] public Transform Sword { get; set; }
        [field: SerializeField] public float SwordForce { get; set; } = 30f;
    }
}