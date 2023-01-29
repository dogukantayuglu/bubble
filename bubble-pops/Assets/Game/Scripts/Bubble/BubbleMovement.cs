using System;
using DG.Tweening;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleMovement : MonoBehaviour
    {
        [SerializeField] private float shootMovementSpeed = 25f;
        [SerializeField] private float gridSlideAnimationDuration = 0.3f;

        private Transform _transform;
        private float _queueAnimationDuration;
        private Action<BubbleEntity> _onBubblePlacedToGrid;
        private BubbleEntity _bubbleEntity;

        public void Initialize(float queueAnimationDuration, Action<BubbleEntity> onBubblePlacedToGrid,
            BubbleEntity bubbleEntity)
        {
            _transform = transform;
            _queueAnimationDuration = queueAnimationDuration;
            _onBubblePlacedToGrid = onBubblePlacedToGrid;
            _bubbleEntity = bubbleEntity;
        }

        public void MoveToCenterPositionOnQueue(Vector3 position)
        {
            _transform.DOScale(Vector3.one * GameData.BubbleSize, _queueAnimationDuration);
            _transform.DOMove(position, _queueAnimationDuration);
        }

        public void GetShotToGrid(GridData targetGrid, Vector3 reflectPoint)
        {
            var targetPosition = targetGrid.Position;
            if (reflectPoint != Vector3.zero)
            {
                MoveToPosition(reflectPoint).OnComplete(() =>
                {
                    MoveToPosition(targetPosition).OnComplete(InvokePlacedOnGridAction);
                });
            }

            else
            {
                MoveToPosition(targetPosition).OnComplete(InvokePlacedOnGridAction);
            }
        }
        
        public void ReAlignToGridPosition(Vector3 position)
        {
            _transform.DOMove(position, gridSlideAnimationDuration);
        }

        private Tween MoveToPosition(Vector3 position)
        {
            return _transform.DOMove(position, shootMovementSpeed).SetSpeedBased();
        }
        
        public Tween MoveToMergePosition(Vector3 targetPosition, float duration)
        {
            return _transform.DOMove(targetPosition, duration);
        }
        
        public void ComeForward(float duration)
        {
            var position = _transform.position;
            position.z = -0.001f;
            _transform.position = position;
            DOVirtual.DelayedCall(duration, GetBackToZeroZPosition);
        }

        private void GetBackToZeroZPosition()
        {
            var position = _transform.position;
            position.z = 0f;
            _transform.position = position;
        }

        private void InvokePlacedOnGridAction()
        {
            _onBubblePlacedToGrid.Invoke(_bubbleEntity);
        }
    }
}