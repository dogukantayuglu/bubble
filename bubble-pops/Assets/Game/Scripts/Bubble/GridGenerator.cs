using System.Collections.Generic;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private int rowCount = 5;
        [SerializeField] private int columnCount = 5;
        [SerializeField] private float yStartPosition;
        [SerializeField] private float columnZigzagValue = 0.5f;
        [SerializeField] private float xOffset = 0.25f;
        [SerializeField] private float verticalSpacingMultiplier = 0.9f;
        [SerializeField] private float horizontalSpacingMultiplier = 1.1f;


        public List<GridData> GenerateGrid()
        {
            var gridDataList = new List<GridData>();
            var xStartPosition = CalculateXStartPosition();

            var position = new Vector2(xStartPosition, yStartPosition);

            for (var i = 0; i < rowCount; i++)
            {
                var zigZagMultiplier = i % 2 == 0 ? 1 : 0;
                var zigZagValue = columnZigzagValue * zigZagMultiplier;

                position.y = yStartPosition - (GameData.BubbleSize * i * verticalSpacingMultiplier);

                for (var j = 0; j < columnCount; j++)
                {
                    position.x = (j * GameData.BubbleSize * horizontalSpacingMultiplier) + xStartPosition + zigZagValue + xOffset;
                    gridDataList.Add(GenerateGridData(i + 1, j + 1, position));
                }

                position.x += columnZigzagValue * zigZagMultiplier;
            }


            return gridDataList;
        }

        private float CalculateXStartPosition()
        {
            var columnCountIsEven = columnCount % 2 == 0;
            var totalColumnInterval = columnCount * GameData.BubbleSize;
            var halfColumnInterval = totalColumnInterval * 0.5f;

            if (columnCountIsEven)
                return (halfColumnInterval - (GameData.BubbleSize * 0.5f)) * -1;
            
            return halfColumnInterval * -1;
        }

        private GridData GenerateGridData(int row, int column, Vector2 position)
        {
            var coordinateData = new GridCoordinateData(row, column);
            var gridData = new GridData(coordinateData, position, GridOccupationStates.Free);

            return gridData;
        }
    }
}