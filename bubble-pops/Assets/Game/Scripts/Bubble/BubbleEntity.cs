using System;
using DG.Tweening;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Interfaces;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleEntity : MonoBehaviour
    {
        public int Value => _value;

        [SerializeField] private BubbleAnimation bubbleAnimation;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro valueText;
        [SerializeField] private float shootMovementSpeed = 15f;
        [SerializeField] private float gridSlideAnimationDuration = 0.5f;

        private IBubblePool _bubblePool;
        private Action<BubbleEntity> _onMovementCompleteAction;
        private Transform _transform;
        private float _queueAnimationDuration;
        private int _value;
        private GridData _gridData;

        public void Initialize(IBubblePool bubblePool, Action<BubbleEntity> onMovementCompleteAction,
            float queueAnimationDuration)
        {
            _bubblePool = bubblePool;
            _queueAnimationDuration = queueAnimationDuration;
            _onMovementCompleteAction = onMovementCompleteAction;
            _transform = transform;
            bubbleAnimation.Initialize();
        }

        public void ActivateOnGrid(GridActivationData gridActivationData)
        {
            SetBubbleValue(gridActivationData.BubbleValueData);
            _transform.position = gridActivationData.GridData.Position;
            _gridData = gridActivationData.GridData;
            _gridData.RegisterBubbleEntity(this);
            gameObject.SetActive(true);
            bubbleAnimation.PlayActivationAnimation();
        }

        public void SetBubbleValue(BubbleValueData bubbleValueData)
        {
            _value = bubbleValueData.value;
            spriteRenderer.color = bubbleValueData.color;
            valueText.text = $"{bubbleValueData.valueText}";
        }

        public void ActivateAtQueue(Vector3 position, bool smallSize)
        {
            _transform.position = position;
            gameObject.SetActive(true);
            bubbleAnimation.PlayActivationAnimation(smallSize);
        }

        public void MoveToCenterPositionOnQueue(Vector3 position)
        {
            _transform.DOScale(Vector3.one * GameData.BubbleSize, _queueAnimationDuration);
            _transform.DOMove(position, _queueAnimationDuration);
        }

        public void GetShotToGrid(GridData targetGrid, Vector3 reflectPoint)
        {
            _gridData = targetGrid;
            _gridData.RegisterBubbleEntity(this);
            var targetPosition = targetGrid.Position;
            if (reflectPoint != Vector3.zero)
            {
                MoveToPosition(reflectPoint).OnComplete(() =>
                {
                    MoveToPosition(targetPosition).OnComplete(() => _onMovementCompleteAction.Invoke(this));
                });
            }

            else
            {
                MoveToPosition(targetPosition).OnComplete(() => _onMovementCompleteAction.Invoke(this));
            }
        }

        private Tween MoveToPosition(Vector3 position)
        {
            return _transform.DOMove(position, shootMovementSpeed).SetSpeedBased();
        }

        public void ReAlignToGridPosition()
        {
            if (_gridData == null) return;
            _transform.DOMove(_gridData.Position, gridSlideAnimationDuration);
        }

        public void ReturnToPool()
        {
            _gridData = null;
            _bubblePool.ReturnBubbleToPool(this);
        }
    }
}