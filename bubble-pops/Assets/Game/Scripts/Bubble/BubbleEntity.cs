using System;
using DG.Tweening;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
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
        [SerializeField] private float queueMovementDuration = 0.25f;

        private bool _isShooting;
        private readonly BubbleCoordinateData _bubbleCoordinate = new();
        private Transform _transform;
        private int _value;

        public void Initialize()
        {
            _transform = transform;
            bubbleAnimation.Initialize();
        }

        public void ActivateOnGrid(GridActivationData gridActivationData)
        {
            SetBubbleValue(gridActivationData.BubbleValueData);
            _transform.position = gridActivationData.ActivationPosition;
            _bubbleCoordinate.Row = gridActivationData.ActivationCoordinateData.Row;
            _bubbleCoordinate.Column = gridActivationData.ActivationCoordinateData.Column;
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

        public Tween MoveToCenterPositionOnQueue(Vector3 position)
        {
            _transform.DOScale(Vector3.one * GameData.BubbleSize, queueMovementDuration);
            return _transform.DOMove(position, queueMovementDuration);
        }

        public void GetShotToGrid(GridData gridData, Vector3 reflectPoint, Action onCompleteAction)
        {
            var targetPosition = gridData.Position;
            if (reflectPoint != Vector3.zero)
            {
                MoveToPosition(reflectPoint).OnComplete(() =>
                {
                    MoveToPosition(targetPosition).OnComplete(onCompleteAction.Invoke);
                });
            }

            else
            {
                MoveToPosition(targetPosition).OnComplete(onCompleteAction.Invoke);
            }
        }

        private Tween MoveToPosition(Vector3 position)
        {
            return _transform.DOMove(position, shootMovementSpeed).SetSpeedBased();
        }
    }
}