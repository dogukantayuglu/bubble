using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

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

        private IGridDataController _gridDataController;

        public void Initialize(IGridDataController gridDataController)
        {
            _gridDataController = gridDataController;
            mergeHandler.Initialize(bubblePool, bubbleThrower.PrepareForThrow);
            bubbleThrower.Initialize(this, gridDataController, queueAnimationDuration);
            bubblePool.Initialize(CheckMerge, queueAnimationDuration);
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
                sequence.AppendCallback(() => GenerateBubbleAtEmptyGrid(randomGridData));
                sequence.AppendInterval(massBubbleGenerationInterval);
                gridDataList.Remove(randomGridData);
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

            var bubbleActivationData = new GridActivationData(
                gridData,
                bubbleValueSo.GetSpawnableValue());

            bubbleEntity.ActivateOnGrid(bubbleActivationData);
            mergeHandler.AddActiveBubble(bubbleEntity);
        }

        public BubbleEntity GetBubbleForPlayer()
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            bubbleEntity.SetBubbleValue(bubbleValueSo.GetSpawnableValue());
            return bubbleEntity;
        }

        private void CheckMerge(BubbleEntity bubbleEntity)
        {
            mergeHandler.CheckMerge(bubbleEntity);
            _gridDataController.RecalculateGrid();
        }
    }
}