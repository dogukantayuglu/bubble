using System.Collections.Generic;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts.Grid
{
    public class GridGenerator : MonoBehaviour
    {
        public int RowCount => rowCount;
        public float TotalVerticalSpacing => GameData.BubbleSize * verticalSpacingMultiplier;

        private float ZigZagValue => _zigZagSwitch ? columnZigzagValue : 0;
        private float TotalHorizontalOffset => _xStartPosition + ZigZagValue + xOffset;
        private float TotalHorizontalSpacing => GameData.BubbleSize * horizontalSpacingMultiplier;

        [SerializeField] private int rowCount = 5;
        [SerializeField] private int columnCount = 5;
        [SerializeField] private float yStartPosition;
        [SerializeField] private float columnZigzagValue = 0.5f;
        [SerializeField] private float xOffset = 0.25f;
        [SerializeField] private float verticalSpacingMultiplier = 0.9f;
        [SerializeField] private float horizontalSpacingMultiplier = 1.1f;
        [SerializeField] private DebugBubbleEntity debugBubbleEntityPrefab;
        [SerializeField] private Transform testBubbleParent;

        private List<GridData> _generatedGridData;
        private List<GridData> _gridDataList;
        private bool _zigZagSwitch;
        private float _xStartPosition;

        private void SwitchZigZagValue() => _zigZagSwitch = !_zigZagSwitch;

        public List<GridData> Initialize()
        {
            _generatedGridData = new List<GridData>();
            _gridDataList = new List<GridData>();
            _xStartPosition = CalculateXStartPosition();
            return GenerateInitialGrid();
        }

        private List<GridData> GenerateInitialGrid()
        {
            var position = new Vector2(_xStartPosition, yStartPosition);

            for (var i = 0; i < rowCount; i++)
            {
                position.y = yStartPosition - (TotalVerticalSpacing * i);
                var rowYPosition = position.y;

                GenerateRow(rowYPosition, i);

                position.x += ZigZagValue;
                SwitchZigZagValue();
            }

            CalculateNeighbours();
            return _gridDataList;
        }

        private void GenerateRow(float rowYPosition, int rowIndex)
        {
            for (var j = 0; j < columnCount; j++)
            {
                var xPos = (j * GameData.BubbleSize * horizontalSpacingMultiplier) + TotalHorizontalOffset;
                var rowPosition = new Vector2(xPos, rowYPosition);
                var gridData = GenerateGridData(rowIndex + 1, j + 1, rowPosition);

                if (!GameManager.GridDebugMode) continue;
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
            CalculateNeighbours();

            SwitchZigZagValue();
        }

        public List<GridData> AddRowFromTop()
        {
            _generatedGridData.Clear();
            var rowStartPosition = yStartPosition + TotalVerticalSpacing;
            GenerateRow(rowStartPosition, -1);
            CalculateNeighbours();

            SwitchZigZagValue();
            return _generatedGridData;
        }

        private void CalculateNeighbours()
        {
            foreach (var centerGridData in _gridDataList)
            {
                var neighbourList = new List<GridData>();
                foreach (var gridData in _gridDataList)
                {
                    var distance = Vector3.Distance(gridData.Position, centerGridData.Position);
                    if (distance <= 0) continue;
                    if (distance <= TotalHorizontalSpacing)
                    {
                        neighbourList.Add(gridData);
                    }
                }

                centerGridData.SetNeighbourList(neighbourList);
            }
        }

        private GridData GenerateGridData(int row, int column, Vector2 position)
        {
            var data = new GridInitializationData(
                row,
                column,
                position,
                GridOccupationStates.Free);

            var gridData = new GridData(data);

            _gridDataList.Add(gridData);
            _generatedGridData.Add(gridData);

            return gridData;
        }
    }
}