using Game.Scripts.Bubble;
using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleBuffer
    {
        BubbleEntity GetBubbleForPlayer();
        void ShootBubble(BubbleEntity bubbleEntity, Vector3 reflectPoint);
    }
}