using Player.Enums;
using UnityEngine;

namespace Player.Commands
{
    public interface IPlayerCommand
    {
        void Execute();
    }

    public interface IPlayerCarryCommand : IPlayerCommand
    {
        public CarryStates CarryState { get; set; }
        GameObject FindNearbyCarriable();
    }

    public interface IPlayerExtendedCommand
    {
        void OnDrawGizmosSelected();
    }

}