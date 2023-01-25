using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleAimHandler
    {
        void HandleBubbleAimHit(RaycastHit hit);
        void DeactivateGhostBubble();
    }
}