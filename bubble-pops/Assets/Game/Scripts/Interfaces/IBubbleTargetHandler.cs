using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleTargetHandler
    {
        void HandleBubbleHit(RaycastHit hit);
        void DeactivateActiveGhostBubble();
    }
}