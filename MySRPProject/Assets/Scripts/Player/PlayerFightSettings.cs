using System;
using Player.Enums;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerFightSettings
    {
        [field: SerializeField] public FightStates FightState { get; set; }
    }
}