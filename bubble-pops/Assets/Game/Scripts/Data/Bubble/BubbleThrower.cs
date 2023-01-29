using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public class BubbleThrower : MonoBehaviour, IBubbleThrower
    {
        [SerializeField] private Transform throwPositionTransform;
        [SerializeField] private int maxQueueCount = 2;
        [SerializeField] private GhostBubbleHandler ghostBubbleHandler;
        [SerializeField] private AimHandler aimHandler;
        [SerializeField] private BubbleValueSo bubbleValueSo;

        private float _queueAnimationDuration;
        private Queue<BubbleEntity> _bubblesInQueue;
        private Vector3 _queueStartPosition;
        private IBubbleBuffer _bubbleBuffer;
        private bool _canThrow;
        private Color _currentColor;

        public void Initialize(IBubbleBuffer bubbleBuffer, IGridDataController gridDataController, float queueAnimDuration)
        {
            _queueAnimationDuration = queueAnimDuration;
            _queueStartPosition = throwPositionTransform.position;
            aimHandler.Initialize(this, _queueStartPosition);
            ghostBubbleHandler.Initialize(gridDataController);
            _bubbleBuffer = bubbleBuffer;
            _bubblesInQueue = new Queue<BubbleEntity>();
            _canThrow = true;
        }

        public void ActivateInitThrowBubbles()
        {
            for (var i = 0; i < maxQueueCount; i++)
            {
                var bubbleEntity = _bubbleBuffer.GetBubbleForPlayer();
                var position = _queueStartPosition;
                position.x -= i * GameData.BubbleSize;
                _bubblesInQueue.Enqueue(bubbleEntity);
                bubbleEntity.ActivateAtQueue(position, i != 0);
            }

            _currentColor = bubbleValueSo.GetColorByValue(_bubblesInQueue.Peek().Value);
        }

        public void ThrowBubble(Vector3 reflectPoint)
        {
            if (!_canThrow) return;
            _canThrow = false;
            var targetGridData = ghostBubbleHandler.TargetGridData;
            if (targetGridData == null)
            {
                CancelThrow();
                return;
            }
            var bubbleEntity = _bubblesInQueue.Dequeue();
            targetGridData.OccupationState = GridOccupationStates.Occupied;
            bubbleEntity.GetShotToGrid(targetGridData, reflectPoint);
        }

        private void CancelThrow()
        {
            ghostBubbleHandler.DeactivateGhostBubble();
            _canThrow = true;
        }

        private void IterateQueue()
        {
            var bubbleEntity = _bubblesInQueue.Peek();
            bubbleEntity.MoveToCenterPositionOnQueue(_queueStartPosition);
            _currentColor = bubbleValueSo.GetColorByValue(bubbleEntity.Value);
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
            ghostBubbleHandler.ActivateGhostBubble(hit, _currentColor);
        }

        public void DeactivateGhostBubble()
        {
            ghostBubbleHandler.DeactivateGhostBubble();
        }

        public void PrepareForThrow()
        {
            if (_bubblesInQueue.Count >= maxQueueCount) return;
            IterateQueue();
            DOVirtual.DelayedCall(_queueAnimationDuration, () => _canThrow = true);
        }
    }
}