using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using Game.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Controllers
{
    public class BubbleController : MonoBehaviour, IBubbleBuffer
    {
        public Action<int> OnNewMergeStarted;

        [SerializeField] private MergeHandler mergeHandler;
        [SerializeField] private BubblePool bubblePool;
        [SerializeField] private BubbleThrower bubbleThrower;
        [SerializeField] private ExplosionHandler explosionHandler;
        [SerializeField] private ActiveBubbleSorter activeBubbleSorter;
        [SerializeField] private BubbleValueSo bubbleValueSo;
        [SerializeField] private float massBubbleGenerationInterval = 0.1f;
        [SerializeField] private float queueAnimationDuration = 0.25f;

        private List<BubbleEntity> _activeBubbleEntities;
        private IGridDataController _gridDataController;

        public void Initialize(IGridDataController gridDataController)
        {
            _activeBubbleEntities = new List<BubbleEntity>();
            _gridDataController = gridDataController;
            activeBubbleSorter.Initialize(_activeBubbleEntities);
            bubbleThrower.Initialize(this, gridDataController, queueAnimationDuration);
            mergeHandler.Initialize(_activeBubbleEntities, bubbleValueSo, MergeCheckComplete, explosionHandler.ExplosionDuration);
            bubblePool.Initialize(StartMergeSequence, queueAnimationDuration, explosionHandler.ExplosionDuration);
            explosionHandler.Initialize(_activeBubbleEntities);
            mergeHandler.OnNewMergeStarted += NotifyNewMergeStarted;
        }

        public void ActivateInitThrowBubbles()
        {
            bubbleThrower.ActivateInitThrowBubbles();
        }

        public void GenerateBubblesForStart()
        {
            var gridDataList = GetInitRowsDataList();
            var sequence = DOTween.Sequence();

            var count = gridDataList.Count;
            for (var i = 0; i < count; i++)
            {
                var randomGridData = gridDataList[Random.Range(0, gridDataList.Count)];
                gridDataList.Remove(randomGridData);
                sequence.AppendCallback(() => GenerateBubbleAtEmptyGrid(randomGridData));
                sequence.AppendInterval(massBubbleGenerationInterval);
            }

            sequence.AppendCallback(activeBubbleSorter.Sort);
        }

        private List<GridData> GetInitRowsDataList()
        {
            var gridDataList = new List<GridData>();

            var gridData = _gridDataController.GetFreeGridData();
            while (gridData.Row <= 4)
            {
                gridDataList.Add(gridData);
                gridData.OccupationState = GridOccupationStates.Occupied;
                gridData = _gridDataController.GetFreeGridData();
            }

            return gridDataList;
        }

        private void GenerateBubbleAtEmptyGrid(GridData gridData)
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            AddBubbleToActiveList(bubbleEntity);

            var bubbleActivationData = new GridActivationData(
                gridData,
                bubbleValueSo.GetSpawnableValue());

            bubbleEntity.ActivateOnGrid(bubbleActivationData);
        }

        public BubbleEntity GetBubbleForPlayer()
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            bubbleEntity.SetBubbleValue(bubbleValueSo.GetSpawnableValue());
            return bubbleEntity;
        }

        public void GenerateBubblesForNewGridData(List<GridData> gridDataList)
        {
            foreach (var gridData in gridDataList)
            {
                gridData.OccupationState = GridOccupationStates.Occupied;
                GenerateBubbleAtEmptyGrid(gridData);
            }

            activeBubbleSorter.Sort();
            gridDataList.Clear();
        }

        private void StartMergeSequence(BubbleEntity bubbleEntity)
        {
            AddBubbleToActiveList(bubbleEntity);
            mergeHandler.CheckMerge(bubbleEntity);
        }

        private void MergeCheckComplete()
        {
            _gridDataController.CheckGridPopulation();

            foreach (var activeBubbleEntity in _activeBubbleEntities)
            {
                activeBubbleEntity.ReAlignToGridPosition();
            }

            DOVirtual.DelayedCall(queueAnimationDuration + 0.1f, bubbleThrower.PrepareForThrow);
        }

        private void HandleBubbleExplosion(BubbleEntity explodedBubbleEntity)
        {
            explosionHandler.HandleBubbleExplosion(explodedBubbleEntity);
        }

        private void AddBubbleToActiveList(BubbleEntity bubbleEntity)
        {
            bubbleEntity.OnBubbleDetachedFromGrid += RemoveBubbleFromActiveList;
            bubbleEntity.OnBubbleExploded += HandleBubbleExplosion;

            _activeBubbleEntities.Add(bubbleEntity);
        }

        private void RemoveBubbleFromActiveList(BubbleEntity bubbleEntity)
        {
            bubbleEntity.OnBubbleDetachedFromGrid -= RemoveBubbleFromActiveList;
            bubbleEntity.OnBubbleExploded -= bubbleEntity.OnBubbleExploded;

            _activeBubbleEntities.Remove(bubbleEntity);
        }

        private void NotifyNewMergeStarted(int mergeCount)
        {
            OnNewMergeStarted?.Invoke(mergeCount);
        }

        private void OnDisable()
        {
            UnsubscribeActions();
        }

        private void UnsubscribeActions()
        {
            mergeHandler.OnNewMergeStarted -= NotifyNewMergeStarted;

            foreach (var activeBubbleEntity in _activeBubbleEntities)
            {
                activeBubbleEntity.OnBubbleDetachedFromGrid -= RemoveBubbleFromActiveList;
                activeBubbleEntity.OnBubbleExploded -= HandleBubbleExplosion;
            }
        }
    }
}