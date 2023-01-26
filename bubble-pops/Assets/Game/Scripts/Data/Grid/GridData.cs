using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Grid
{
    public class GridData
    {
        public Vector2 Position { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public GridOccupationStates OccupationState;
        public DebugBubbleEntity DebugBubbleEntity;

        private Vector2 _position;

        public GridData(int row, int column, Vector2 position, GridOccupationStates occupationState)
        {
            SetCoordinates(row, column);
            Position = position;
            OccupationState = occupationState;
        }
        
        public void SetCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void SetPosition(Vector2 targetPosition)
        {
            Position = targetPosition;
            DebugBubbleEntity.SnapToGrid(this);
        }
    }
}
