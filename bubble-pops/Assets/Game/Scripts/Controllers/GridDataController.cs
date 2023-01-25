using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GridDataController : MonoBehaviour, IGridDataProvider
    {
        [SerializeField] private BubbleGridGenerator bubbleGridGenerator;
        [SerializeField] private RectTransform gridBackgroundTransform;

        private List<GridData> _gridDataList;

        public void Initialize()
        {
            _gridDataList = bubbleGridGenerator.GenerateGrid();
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
    }
}