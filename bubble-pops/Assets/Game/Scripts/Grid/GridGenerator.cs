using System.Collections.Generic;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Grid
{
    public class GridGenerator : MonoBehaviour
    {
        public int RowCount => rowCount;
        public float TotalVerticalSpacing => GameData.BubbleSize * verticalSpacingMultiplier;
        public float TotalHorizontalSpacing => GameData.BubbleSize * horizontalSpacingMultiplier;

        private float ZigZagValue => _zigZagSwitch ? columnZigzagValue : 0;
        private float TotalHorizontalOffset => _xStartPosition + ZigZagValue + xOffset;

        [SerializeField] private int rowCount = 5;
        [SerializeField] private int columnCount = 5;
        [SerializeField] private float yStartPosition;
        [SerializeField] private float columnZigzagValue = 0.5f;
        [SerializeField] private float xOffset = 0.25f;
        [SerializeField] private float verticalSpacingMultiplier = 0.9f;
        [SerializeField] private float horizontalSpacingMultiplier = 1.1f;
        [SerializeField] private DebugBubbleEntity debugBubbleEntityPrefab;
        [SerializeField] private Transform testBubbleParent;

        private List<GridData> _gridDataList;
        private bool _zigZagSwitch;
        private float _xStartPosition;

        private void SwitchZigZagValue() => _zigZagSwitch = !_zigZagSwitch;

        public List<GridData> GenerateInitialGrid()
        {
            _gridDataList = new List<GridData>();
            _xStartPosition = CalculateXStartPosition();

            var position = new Vector2(_xStartPosition, yStartPosition);

            for (var i = 0; i < rowCount; i++)
            {
                position.y = yStartPosition - (TotalVerticalSpacing * i);
                var rowYPosition = position.y;

                GenerateRow(rowYPosition, i);

                position.x += ZigZagValue;
                SwitchZigZagValue();
            }


            return _gridDataList;
        }

        private void GenerateRow(float rowYPosition, int rowIndex)
        {
            for (var j = 0; j < columnCount; j++)
            {
                var xPos = (j * GameData.BubbleSize * horizontalSpacingMultiplier) + TotalHorizontalOffset;
                var rowPosition = new Vector2(xPos, rowYPosition);
                var gridData = GenerateGridData(rowIndex + 1, j + 1, rowPosition);
                
                //Debug
                var debugBubble = Instantiate(debugBubbleEntityPrefab, rowPosition, Quaternion.identity,
                    testBubbleParent);
                debugBubble.SnapToGrid(gridData);
                gridData.DebugBubbleEntity = debugBubble;
            }
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

        public void AddRowFromBottom()
        {
            var rowYPosition = yStartPosition - (GameData.BubbleSize * rowCount * verticalSpacingMultiplier);
            
            GenerateRow(rowYPosition, rowCount);
            
            SwitchZigZagValue();
        }

        public void AddRowFromTop()
        {
            var rowStartPosition = yStartPosition + TotalVerticalSpacing;
            GenerateRow(rowStartPosition, -1);
            
            SwitchZigZagValue();
        }

        private GridData GenerateGridData(int row, int column, Vector2 position)
        {
            var gridData = new GridData(row, column, position, GridOccupationStates.Free);
            _gridDataList.Add(gridData);

            return gridData;
        }
    }
}