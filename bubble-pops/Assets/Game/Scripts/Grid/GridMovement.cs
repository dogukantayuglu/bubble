using System.Collections.Generic;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Grid
{
    public class GridMovement : MonoBehaviour
    {
        private List<GridData> _gridDataList;
        private float _verticalMovementStep;
        private int _maxRowCount;

        public void Initialize(List<GridData> gridDataList, float verticalMovementStep, int maxRowCount)
        {
            _gridDataList = gridDataList;
            _verticalMovementStep = verticalMovementStep;
            _maxRowCount = maxRowCount;
        }
        public void MoveGridToUp()
        {
            var listToDestroy = new List<GridData>();
            foreach (var gridData in _gridDataList)
            {
                var position = gridData.Position;
                position.y += _verticalMovementStep;
                var targetRow = gridData.Row - 1;
                
                if (targetRow < 1)
                {
                    listToDestroy.Add(gridData);
                }
                else
                {
                    UpdateGridRow(gridData, position, targetRow);
                }
            }

            DestroyListedGridData(listToDestroy);
        }

        public void MoveGridDown()
        {
            var listToDestroy = new List<GridData>();
            
            foreach (var gridData in _gridDataList)
            {
                var position = gridData.Position;
                position.y -= _verticalMovementStep;
                var targetRow = gridData.Row + 1;
                
                if (targetRow > _maxRowCount)
                {
                    listToDestroy.Add(gridData);
                }
                else
                {
                    UpdateGridRow(gridData, position, targetRow);
                }
            }

            DestroyListedGridData(listToDestroy);
        }
        
        private void DestroyListedGridData(List<GridData> listToDestroy)
        {
            foreach (var gridData in listToDestroy)
            {
                gridData.RemoveBubbleFromGrid();
                _gridDataList.Remove(gridData);
            }
        }

        private void UpdateGridRow(GridData gridData, Vector2 position, int row)
        {
            gridData.SetCoordinates(row, gridData.Column);
            gridData.SetPosition(position);
        }
    }
}
