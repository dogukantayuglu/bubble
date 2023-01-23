using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public IBubbleBuffer BubbleBuffer { get; set; }
        
        [SerializeField] private Transform queueStartPosition;
        [SerializeField] private float horizontalQueueSpace = 0.5f;
        [SerializeField] private int maxQueueCount = 2;

        private Queue<BubbleEntity> _bubblesInQueue;

        public void Initialize()
        {
            _bubblesInQueue = new Queue<BubbleEntity>();
        }

        public void AddBubbleToQueue()
        {
            var freeSlots = maxQueueCount - _bubblesInQueue.Count;

            for (var i = 0; i < freeSlots; i++)
            {
                _bubblesInQueue.Enqueue(BubbleBuffer.GetBubbleForPlayer());
            }
        }
        
    }
}
