using System.Collections.Generic;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleGridGenerator : MonoBehaviour
    {
        [SerializeField] private int rowCount = 5;
        [SerializeField] private int columnCount = 5;
        [SerializeField] private float yStartPosition;
        [SerializeField] private float columnZigzagValue = 0.5f;
        [SerializeField] private float columnSpaceInterval = 1f;
        [SerializeField] private float rowSpaceInterval = 1f;
        [SerializeField] private float xOffset = 0.25f;


        public List<GridData> GenerateGrid()
        {
            var gridDataList = new List<GridData>();
            var xStartPosition = CalculateXStartPosition();

            var position = new Vector2(xStartPosition, yStartPosition);

            for (var i = 0; i < rowCount; i++)
            {
                var zigZagMultiplier = i % 2 == 0 ? 1 : 0;
                var zigZagValue = columnZigzagValue * zigZagMultiplier;

                position.y = yStartPosition - (rowSpaceInterval * i);

                for (var j = 0; j < columnCount; j++)
                {
                    position.x = (j * columnSpaceInterval) + xStartPosition + zigZagValue + xOffset;
                    gridDataList.Add(GenerateGridData(i, j, position));
                }

                position.x += columnZigzagValue * zigZagMultiplier;
            }


            return gridDataList;
        }

        private float CalculateXStartPosition()
        {
            var columnCountIsEven = columnCount % 2 == 0;
            var totalColumnInterval = columnCount * columnSpaceInterval;
            var halfColumnInterval = totalColumnInterval * 0.5f;

            if (columnCountIsEven)
            {
                return (halfColumnInterval - (columnSpaceInterval * 0.5f)) * -1;
            }

            return halfColumnInterval * -1;
        }

        private GridData GenerateGridData(int row, int column, Vector2 position)
        {
            var coordinateData = new CoordinateData(row, column);
            var gridData = new GridData(coordinateData, position, GridOccupationStates.Free);

            return gridData;
        }
    }
}