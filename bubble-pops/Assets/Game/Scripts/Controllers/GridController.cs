using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GridController : MonoBehaviour, IGridDataController
    {
        [SerializeField] private GridGenerator gridGenerator;
        [SerializeField] private RectTransform gridBackgroundTransform;

        private List<GridData> _gridDataList;
        private int _maxRowCount;
        private int _minRowCount;

        public void Initialize()
        {
            _gridDataList = gridGenerator.GenerateInitialGrid();
            _maxRowCount = gridGenerator.RowCount;
            _minRowCount = gridGenerator.RowCount - 2;
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

        public void RecalculateGrid()
        {
            var activeRowCount = 0;
            foreach (var gridData in _gridDataList)
            {
                if (gridData.OccupationState != GridOccupationStates.Occupied) continue;
                if (gridData.Row > activeRowCount)
                    activeRowCount = gridData.Row;
            }
            
            if(activeRowCount >= _maxRowCount)
                AddRowFromBottom();
            else if(activeRowCount <= _minRowCount)
                AddRowFromTop();
        }

        private void AddRowFromBottom()
        {
            gridGenerator.AddRowFromBottom();
            MoveGridToUp();
            print("Bottom");
        }

        private void MoveGridToUp()
        {
            for (var i = 0; i < _gridDataList.Count; i++)
            {
                var gridData = _gridDataList[i];
                var position = gridData.Position;
                position.y += gridGenerator.TotalVerticalSpacing;
                gridData.SetCoordinates(gridData.Row - 1, gridData.Column);
                gridData.SetPosition(position);
            }
        }

        private void AddRowFromTop()
        {
            print("Top");
        }
    }
}