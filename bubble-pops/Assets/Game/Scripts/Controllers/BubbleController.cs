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
        [SerializeField] private MergeHandler mergeHandler;
        [SerializeField] private BubblePool bubblePool;
        [SerializeField] private BubbleValueSo bubbleValueSo;
        [SerializeField] private BubbleThrower bubbleThrower;
        [SerializeField] private float massBubbleGenerationInterval = 0.1f;
        [SerializeField] private float queueAnimationDuration = 0.25f;

        private List<BubbleEntity> _activeBubbleEntities;
        private IGridDataController _gridDataController;

        public void Initialize(IGridDataController gridDataController)
        {
            _activeBubbleEntities = new List<BubbleEntity>();
            _gridDataController = gridDataController;
            bubbleThrower.Initialize(this, gridDataController, queueAnimationDuration);
            mergeHandler.Initialize(_activeBubbleEntities, bubbleValueSo, MergeCheckComplete);
            bubblePool.Initialize(StartMergeSequence, queueAnimationDuration);
            bubblePool.OnBubbleReturnedPool += RemoveBubbleFromActiveList;
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
            _activeBubbleEntities.Add(bubbleEntity);

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
            
            gridDataList.Clear();
        }

        private void StartMergeSequence(BubbleEntity bubbleEntity)
        {
            _activeBubbleEntities.Add(bubbleEntity);
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

        private void RemoveBubbleFromActiveList(BubbleEntity bubbleEntity)
        {
            _activeBubbleEntities.Remove(bubbleEntity);
        }

        private void OnDisable()
        {
            bubblePool.OnBubbleReturnedPool -= RemoveBubbleFromActiveList;
        }
    }
}