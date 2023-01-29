using System;
using DG.Tweening;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using Game.Scripts.Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Bubble
{
    public class BubbleEntity : MonoBehaviour
    {
        public Action<BubbleEntity> OnBubbleDetachedFromGrid;

        public bool IsConnectedToGrid
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                if (_gridData == null) return;
                if (_gridData.DebugBubbleEntity)
                    _gridData.DebugBubbleEntity.SetIsConnected(value);
            }
        }

        public int Value => _value;
        public GridData GridData => _gridData;

        [SerializeField] private BubbleAnimation bubbleAnimation;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro valueText;
        [SerializeField] private float shootMovementSpeed = 15f;
        [SerializeField] private float gridSlideAnimationDuration = 0.5f;
        [SerializeField] private GameObject outerCircle;

        [SerializeField] private float fallXPositionRandomAmount = 0.5f;
        [SerializeField] private float fallJumpRandomAmount = 0.5f;
        [SerializeField] private float dropDuration = 1f;

        private IBubblePool _bubblePool;
        private Action<BubbleEntity> _onBubblePlacedToGrid;
        private GridData _gridData;
        private Transform _transform;
        private float _queueAnimationDuration;
        private int _value;
        private bool _isConnected;

        public void Initialize(IBubblePool bubblePool, Action<BubbleEntity> onBubblePlacedToGrid,
            float queueAnimationDuration)
        {
            if (GameManager.GridDebugMode)
                valueText.gameObject.SetActive(false);

            _bubblePool = bubblePool;
            _queueAnimationDuration = queueAnimationDuration;
            _onBubblePlacedToGrid = onBubblePlacedToGrid;
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
            outerCircle.gameObject.SetActive(_value > 512);
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
                    MoveToPosition(targetPosition).OnComplete(InvokePlacedOnGridAction);
                });
            }

            else
            {
                MoveToPosition(targetPosition).OnComplete(InvokePlacedOnGridAction);
            }
        }

        private void InvokePlacedOnGridAction()
        {
            _onBubblePlacedToGrid.Invoke(this);
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

        public void MergeToPosition(Vector3 targetPosition, float duration)
        {
            DetachFromGridData();
            _transform.DOMove(targetPosition, duration).OnComplete(ReturnToPool);
        }

        private void ReturnToPool()
        {
            _bubblePool.ReturnBubbleToPool(this);
        }

        public void DropFromGrid()
        {
            DetachFromGridData();
            var currentPosition = _transform.position;
            var randomXTarget = currentPosition.x +
                                Random.Range(-fallXPositionRandomAmount, fallXPositionRandomAmount);
            var randomJumpPower = Random.Range(0, fallJumpRandomAmount);
            var targetPosition = new Vector3(randomXTarget, -6, currentPosition.z);
            _transform.DOJump(targetPosition, randomJumpPower, 1, dropDuration).OnComplete(ReturnToPool);
        }

        public void OnGridDestroyed()
        {
            DetachFromGridData();
            ReturnToPool();
        }

        private void DetachFromGridData()
        {
            _gridData.OccupationState = GridOccupationStates.Free;
            _gridData.UnRegisterBubbleEntity(this);
            _gridData = null;
            OnBubbleDetachedFromGrid?.Invoke(this);
        }

        [ContextMenu("PrintGridData")]
        public void PrintGridData()
        {
            print($"{_gridData.Row} {_gridData.Column}");
            print($"{_gridData.BubbleEntity}");
            print($"_IsConnected: {_gridData.BubbleEntity.IsConnectedToGrid}");
            print($"{_gridData.OccupationState}");
            foreach (var data in _gridData.NeighbourGridDataList)
            {
                print("Neighbour");
                print($"{data.Row} {data.Column}");
                if (data.BubbleEntity)
                {
                    print($"{data.BubbleEntity}");
                    print($"_IsConnected: {data.BubbleEntity.IsConnectedToGrid}");
                }
                print($"{data.OccupationState}");
                print("------------------");
            }
        }
    }
}