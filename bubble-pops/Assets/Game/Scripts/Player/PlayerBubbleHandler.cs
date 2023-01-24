using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerBubbleHandler : MonoBehaviour
    {
        public IBubbleBuffer BubbleBuffer { get; set; }
        
        [SerializeField] private int maxQueueCount = 2;

        private List<BubbleEntity> _bubblesInQueue;
        private Vector3 _queueStartPosition;
        
        public void Initialize(Vector3 queueStartPosition)
        {
            _queueStartPosition = queueStartPosition;
            _bubblesInQueue = new List<BubbleEntity>();
        }

        public void ActivateInitBubbles()
        {
            for (var i = 0; i < maxQueueCount; i++)
            {
                var bubbleEntity = BubbleBuffer.GetBubbleForPlayer();
                var position = _queueStartPosition;
                position.x -= i * GameData.BubbleSize;
                _bubblesInQueue.Add(bubbleEntity);
                bubbleEntity.ActivateAtQueue(position, i != 0);
            }
        }
    }
}
