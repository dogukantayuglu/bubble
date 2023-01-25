using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class BubbleThrower : MonoBehaviour, IBubbleThrower
    {
        [SerializeField] private int maxQueueCount = 2;
        [SerializeField] private GhostBubbleHandler ghostBubbleHandler;

        private Queue<BubbleEntity> _bubblesInQueue;
        private Vector3 _queueStartPosition;
        private IBubbleBuffer _bubbleBuffer;
        private bool _canThrow;

        public void Initialize(IBubbleBuffer bubbleBuffer, IGridDataProvider gridDataProvider, Vector3 queueStartPosition)
        {
            ghostBubbleHandler.Initialize(gridDataProvider);
            _bubbleBuffer = bubbleBuffer;
            _queueStartPosition = queueStartPosition;
            _bubblesInQueue = new Queue<BubbleEntity>();
            _canThrow = true;
        }

        public void ActivateInitBubbles()
        {
            for (var i = 0; i < maxQueueCount; i++)
            {
                var bubbleEntity = _bubbleBuffer.GetBubbleForPlayer();
                var position = _queueStartPosition;
                position.x -= i * GameData.BubbleSize;
                _bubblesInQueue.Enqueue(bubbleEntity);
                bubbleEntity.ActivateAtQueue(position, i != 0);
            }
        }

        public void ThrowBubble(Vector3 reflectPoint)
        {
            if (!_canThrow) return;
            DelayThrowing();
            var bubbleEntity = _bubblesInQueue.Dequeue();
            var targetGridData = ghostBubbleHandler.TargetGridData;
            targetGridData.OccupationState = GridOccupationStates.Occupied;
            bubbleEntity.GetShotToGrid(targetGridData, reflectPoint);
            ghostBubbleHandler.DeactivateGhostBubble();
            IterateQueue();
        }

        private void DelayThrowing()
        {
            _canThrow = false;
            DOVirtual.DelayedCall(GameData.QueueDelay, () => _canThrow = true);
        }

        private void IterateQueue()
        {
            var bubbleEntity = _bubblesInQueue.Peek();
            bubbleEntity.MoveToCenterPositionOnQueue(_queueStartPosition);
            AddItemToQueue();
        }

        private void AddItemToQueue()
        {
            var bubbleEntity = _bubbleBuffer.GetBubbleForPlayer();
            var position = _queueStartPosition;
            position.x -= _bubblesInQueue.Count * GameData.BubbleSize;
            _bubblesInQueue.Enqueue(bubbleEntity);
            bubbleEntity.ActivateAtQueue(position, true);
        }

        public void ActivateGhostBubble(RaycastHit hit)
        {
            ghostBubbleHandler.ActivateGhostBubble(hit);
        }

        public void DeactivateGhostBubble()
        {
            ghostBubbleHandler.DeactivateGhostBubble();
        }
    }
}