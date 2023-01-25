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
    public class BubbleController : MonoBehaviour, IBubbleBuffer, IBubbleTargetHandler
    {
        [SerializeField] private BubblePool bubblePool;
        [SerializeField] private BubbleValueSo bubbleValueSo;
        [SerializeField] private GhostBubbleEntity ghostBubbleEntityPrefab;
        [SerializeField] private float massBubbleGenerationInterval = 0.1f;

        private List<BubbleEntity> _activeBubbleEntities;
        private GhostBubbleEntity _ghostBubbleEntity;
        private IGridBuffer _gridBuffer;

        public void Initialize(IGridBuffer gridBuffer)
        {
            GenerateGhostBubble();
            _gridBuffer = gridBuffer;
            _activeBubbleEntities = new List<BubbleEntity>();
            bubblePool.Initialize();
        }

        private void GenerateGhostBubble()
        {
            _ghostBubbleEntity = Instantiate(ghostBubbleEntityPrefab, Vector3.zero, Quaternion.identity);
            _ghostBubbleEntity.Initialize();
        }
        
        public void GenerateBubblesForStart()
        {
            var gridDataList = GetInitRowsDataList();
            var sequence = DOTween.Sequence();
            
            var count = gridDataList.Count;
            for (var i = 0; i < count; i++)
            {
                var randomGridData = gridDataList[Random.Range(0, gridDataList.Count)];
                sequence.AppendCallback(()=> GenerateBubbleAtEmptyGrid(randomGridData));
                sequence.AppendInterval(massBubbleGenerationInterval);
                gridDataList.Remove(randomGridData);
            }
        }

        private List<GridData> GetInitRowsDataList()
        {
            var gridDataList = new List<GridData>();

            var gridData = _gridBuffer.GetFreeGridData();
            while (gridData.GridCoordinateData.Row <= 4)
            {
                gridDataList.Add(gridData);
                gridData.OccupationState = GridOccupationStates.Occupied;
                gridData = _gridBuffer.GetFreeGridData();
            }

            return gridDataList;
        }

        private void GenerateBubbleAtEmptyGrid(GridData gridData)
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            
            var bubbleActivationData = new GridActivationData(
                gridData.GridCoordinateData, 
                gridData.Position,
                bubbleValueSo.GetSpawnableValue());
            
            bubbleEntity.ActivateOnGrid(bubbleActivationData);
            _activeBubbleEntities.Add(bubbleEntity);
        }

        public BubbleEntity GetBubbleForPlayer()
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            bubbleEntity.SetBubbleValue(bubbleValueSo.GetSpawnableValue());
            return bubbleEntity;
        }

        public void HandleBubbleHit(RaycastHit hit)
        {
            var closestGridData = _gridBuffer.GetClosesFreeGridData(hit.point);
            _ghostBubbleEntity.ActivateAtGrid(closestGridData);
        }

        public void DeactivateActiveGhostBubble()
        {
            _ghostBubbleEntity.Deactivate();
        }
    }
}