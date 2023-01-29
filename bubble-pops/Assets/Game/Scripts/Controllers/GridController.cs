using System;
using System.Collections.Generic;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Grid;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GridController : MonoBehaviour, IGridDataController
    {
        public Action OnAllGridCleared;
        
        [SerializeField] private GridGenerator gridGenerator;
        [SerializeField] private GridMovement gridMovement;
        [SerializeField] private RectTransform gridBackgroundTransform;

        private IBubbleBuffer _bubbleBuffer;
        private List<GridData> _gridDataList;
        private int _maxRowCount;
        private int _minRowCount;

        public void Initialize(IBubbleBuffer bubbleBuffer)
        {
            _maxRowCount = gridGenerator.RowCount;
            _minRowCount = gridGenerator.RowCount - 2;
            
            _gridDataList = gridGenerator.Initialize();
            _bubbleBuffer = bubbleBuffer;
            gridMovement.Initialize(_gridDataList, gridGenerator.TotalVerticalSpacing, _maxRowCount);
            SetGridBackgroundSize();
        }

        private void SetGridBackgroundSize()
        {
            var sizeDelta = gridBackgroundTransform.sizeDelta;
            sizeDelta.x *= GameData.BubbleSize;
            gridBackgroundTransform.sizeDelta = sizeDelta;
        }

        public GridData GetFreeGridData()
        {
            foreach (var gridData in _gridDataList)
            {
                if (gridData.OccupationState == GridOccupationStates.Free)
                {
                    return gridData;
                }
            }

            return null;
        }

        public GridData GetClosestFreeGridData(Vector2 position)
        {
            var closestGridData = GetFreeGridData();
            var closestPosition = closestGridData.Position;
            var closestDistance = Vector2.Distance(closestPosition, position);

            foreach (var gridData in _gridDataList)
            {
                if (gridData.OccupationState != GridOccupationStates.Free) continue;
                var distance = Vector2.Distance(gridData.Position, position);
                if (distance >= closestDistance) continue;
                closestDistance = distance;
                closestGridData = gridData;
            }

            return closestGridData;
        }

        public void CheckGridPopulation()
        {
            var occupiedGridCount = 0;
            var activeRowCount = 0;
            foreach (var gridData in _gridDataList)
            {
                if (gridData.OccupationState != GridOccupationStates.Occupied) continue;
                occupiedGridCount++;
                if (gridData.Row > activeRowCount)
                    activeRowCount = gridData.Row;
            }

            if (occupiedGridCount < gridGenerator.ColumnCount)
            {
                HandleAllGridCleared();
                return;
            }

            if (activeRowCount >= _maxRowCount)
                AddRowFromBottom();
            else if (activeRowCount <= _minRowCount)
                AddRowFromTop();
        }

        private void AddRowFromBottom()
        {
            gridGenerator.AddRowFromBottom();
            gridMovement.MoveGridToUp();
        }

        private void AddRowFromTop()
        {
            var generatedList = gridGenerator.AddRowFromTop();
            _bubbleBuffer.GenerateBubblesForNewGridData(generatedList);
            gridMovement.MoveGridDown();
        }

        private void HandleAllGridCleared()
        {
            OnAllGridCleared?.Invoke();

            for (var i = 0; i < 4; i++)
            {
                AddRowFromTop();
            }
        }
    }
}