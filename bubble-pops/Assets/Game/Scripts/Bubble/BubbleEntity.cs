using System;
using DG.Tweening;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleEntity : MonoBehaviour
    {
        public Action<BubbleEntity> OnBubbleDetachedFromGrid;
        public Action<BubbleEntity> OnBubbleExploded;

        public bool IsConnectedToGrid
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                if (GridData == null) return;
                if (GridData.DebugBubbleEntity)
                    GridData.DebugBubbleEntity.SetIsConnected(value);
            }
        }

        public int Value { get; private set; }
        public GridData GridData { get; private set; }

        [SerializeField] private BubbleMovement bubbleMovement;
        [SerializeField] private BubbleAnimation bubbleAnimation;
        [SerializeField] private BubbleVisual bubbleVisual;

        private IBubblePool _bubblePool;
        private Transform _transform;
        private bool _isConnected;

        public void Initialize(IBubblePool bubblePool, Action<BubbleEntity> onBubblePlacedToGrid,
            float queueAnimationDuration, float explosionDuration)
        {
            _bubblePool = bubblePool;
            _transform = transform;
            bubbleMovement.Initialize(queueAnimationDuration, onBubblePlacedToGrid, this);
            bubbleAnimation.Initialize(explosionDuration);
            bubbleVisual.Initialize();
        }

        public void ActivateOnGrid(GridActivationData gridActivationData)
        {
            SetBubbleValue(gridActivationData.BubbleValueData);
            _transform.position = gridActivationData.GridData.Position;
            GridData = gridActivationData.GridData;
            GridData.RegisterBubbleEntity(this);
            gameObject.SetActive(true);
            bubbleAnimation.ActivationAnimation();
        }

        public void SetBubbleValue(BubbleValueData bubbleValueData)
        {
            Value = bubbleValueData.value;
            bubbleVisual.SetBubbleVisual(bubbleValueData);
        }

        public void ActivateAtQueue(Vector3 position, bool smallSize)
        {
            _transform.position = position;
            gameObject.SetActive(true);
            bubbleAnimation.ActivationAnimation(smallSize);
        }

        public void MoveToCenterPositionOnQueue(Vector3 position)
        {
            bubbleMovement.MoveToCenterPositionOnQueue(position);
        }

        public void GetShotToGrid(GridData targetGrid, Vector3 reflectPoint)
        {
            GridData = targetGrid;
            GridData.RegisterBubbleEntity(this);
            bubbleMovement.GetShotToGrid(targetGrid, reflectPoint);
        }
        
        public void ReAlignToGridPosition()
        {
            if (GridData == null) return;
            bubbleMovement.ReAlignToGridPosition(GridData.Position);
        }

        public void MoveToMergePosition(Vector3 targetPosition, float duration)
        {
            DetachFromGridData();
            bubbleMovement.MoveToMergePosition(targetPosition, duration).OnComplete(ReturnToPool);
            bubbleVisual.FadeOut(duration);
        }
        
        public void PrepareToGetMerged(float duration)
        {
            bubbleMovement.ComeForward(duration);
        }

        public void Explode()
        {
            bubbleAnimation.PlayExplodeAnimation().OnComplete(ReturnToPool);
            OnBubbleExploded?.Invoke(this);
            DetachFromGridData();
        }
        
        public void GetAffectedFromExplosion(Vector3 explosionOrigin)
        {
            DetachFromGridData();
            bubbleAnimation.PlayAffectionFromExplosionAnimation(explosionOrigin).OnComplete(ReturnToPool);
        }

        private void ReturnToPool()
        {
            _bubblePool.ReturnBubbleToPool(this);
        }

        public void DropFromGrid()
        {
            DetachFromGridData();
            bubbleAnimation.DropAnimation().OnComplete(ReturnToPool);
        }

        public void OnGridDestroyed()
        {
            DetachFromGridData();
            ReturnToPool();
        }

        private void DetachFromGridData()
        {
            GridData.OccupationState = GridOccupationStates.Free;
            GridData.UnRegisterBubbleEntity(this);
            GridData = null;
            OnBubbleDetachedFromGrid?.Invoke(this);
        }
    }
}