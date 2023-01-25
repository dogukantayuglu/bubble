using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleAimTarget
    {
        void ActivateGhostBubble(RaycastHit hit);
        void DeactivateGhostBubble();
    }
}