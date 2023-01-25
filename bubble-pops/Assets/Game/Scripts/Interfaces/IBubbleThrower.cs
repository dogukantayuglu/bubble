
using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleThrower
    {
        void ThrowBubble(Vector3 reflectPoint);
        void ActivateGhostBubble(RaycastHit hit);
        void DeactivateGhostBubble();
    }
}