using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerBubbleHandler : MonoBehaviour
    {
        [SerializeField] private int maxQueueCount = 2;

        private List<BubbleEntity> _bubblesInQueue;
        private Vector3 _queueStartPosition;
        private IBubbleBuffer _bubbleBuffer;

        public void Initialize(IBubbleBuffer bubbleBuffer, Vector3 queueStartPosition)
        {
            _bubbleBuffer = bubbleBuffer;
            _queueStartPosition = queueStartPosition;
            _bubblesInQueue = new List<BubbleEntity>();
        }

        public void ActivateInitBubbles()
        {
            for (var i = 0; i < maxQueueCount; i++)
            {
                var bubbleEntity = _bubbleBuffer.GetBubbleForPlayer();
                var position = _queueStartPosition;
                position.x -= i * GameData.BubbleSize;
                _bubblesInQueue.Add(bubbleEntity);
                bubbleEntity.ActivateAtQueue(position, i != 0);
            }
        }
    }
}
