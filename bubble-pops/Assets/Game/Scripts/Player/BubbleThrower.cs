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
        public IBubbleAimTarget BubbleAimTarget => ghostBubbleHandler;
        [SerializeField] private int maxQueueCount = 2;
        [SerializeField] private GhostBubbleHandler ghostBubbleHandler;

        private Queue<BubbleEntity> _bubblesInQueue;
        private Vector3 _queueStartPosition;
        private IBubbleBuffer _bubbleBuffer;
        private bool _canThrow;

        public void Initialize(IBubbleBuffer bubbleBuffer, IGridBuffer gridBuffer, Vector3 queueStartPosition)
        {
            ghostBubbleHandler.Initialize(gridBuffer);
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
            _canThrow = false;
            var bubbleEntity = _bubblesInQueue.Dequeue();
            var targetGridData = ghostBubbleHandler.TargetGridData;
            targetGridData.OccupationState = GridOccupationStates.Occupied;
            bubbleEntity.GetShotToGrid(targetGridData, reflectPoint, EnableThrowing);
            MoveItemInQueue();
        }

        private void EnableThrowing()
        {
            _canThrow = true;
        }

        private void MoveItemInQueue()
        {
            var bubbleEntity = _bubblesInQueue.Peek();
            bubbleEntity.MoveToCenterPositionOnQueue(_queueStartPosition).OnComplete(AddItemToQueue);
        }

        private void AddItemToQueue()
        {
            var bubbleEntity = _bubbleBuffer.GetBubbleForPlayer();
            var position = _queueStartPosition;
            position.x -= _bubblesInQueue.Count * GameData.BubbleSize;
            _bubblesInQueue.Enqueue(bubbleEntity);
            bubbleEntity.ActivateAtQueue(position, true);
        }
    }
}