using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public IBubbleBuffer BubbleBuffer { get; set; }
        
        [SerializeField] private Transform queueStartTransform;
        [SerializeField] private float horizontalQueueSpace = 0.5f;
        [SerializeField] private int maxQueueCount = 2;

        private List<BubbleEntity> _bubblesInQueue;

        public void Initialize()
        {
            _bubblesInQueue = new List<BubbleEntity>();
        }

        public void ActivateInitBubbles()
        {
            for (var i = 0; i < maxQueueCount; i++)
            {
                var bubbleEntity = BubbleBuffer.GetBubbleForPlayer();
                var position = queueStartTransform.position;
                position.x -= i * horizontalQueueSpace;
                queueStartTransform.position = position;
                _bubblesInQueue.Add(bubbleEntity);
                bubbleEntity.ActivateAtQueue(position, i != 0);
            }
        }

    }
}
