using System;
using Game.Scripts.Bubble;
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

        [SerializeField]
        private BubbleEntity _bubbleEntity;

        public GridData(int row, int column, Vector2 position, GridOccupationStates occupationState)
        {
            SetCoordinates(row, column);
            SetPosition(position);
            OccupationState = occupationState;
        }

        public void RegisterBubbleEntity(BubbleEntity bubbleEntity)
        {
            _bubbleEntity = bubbleEntity;
        }

        public void RemoveBubbleFromGrid()
        {
            if (_bubbleEntity)
            {
                _bubbleEntity.ReturnToPool();
                _bubbleEntity = null;
            }

            if (DebugBubbleEntity)
            {
                DebugBubbleEntity.GetDestroyed();
            }
        }

        public void SetCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void SetPosition(Vector2 targetPosition)
        {
            Position = targetPosition;
            
            if (DebugBubbleEntity)
                DebugBubbleEntity.SnapToGrid(this);
            if (_bubbleEntity)
                _bubbleEntity.FollowRegisteredGrid(targetPosition);
        }
    }
}