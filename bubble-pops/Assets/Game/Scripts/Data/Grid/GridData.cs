using System.Collections.Generic;
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
        public BubbleEntity BubbleEntity { get; private set; }
        public List<GridData> NeighbourGridDataList { get; private set; }
        public GridOccupationStates OccupationState;
        public DebugBubbleEntity DebugBubbleEntity;
        
        public GridData(GridInitializationData gridInitializationData)
        {
            SetCoordinates(gridInitializationData.Row, gridInitializationData.Column);
            SetPosition(gridInitializationData.Position);
            OccupationState = gridInitializationData.GridOccupationState;
        }

        public void RegisterBubbleEntity(BubbleEntity bubbleEntity)
        {
            BubbleEntity = bubbleEntity;
        }

        public void UnRegisterBubbleEntity(BubbleEntity bubbleEntity)
        {
            if (bubbleEntity != BubbleEntity) return;
            DebugBubbleEntity.SetIsConnected(true);
            BubbleEntity = null;
        }

        public void RemoveBubbleFromGrid()
        {
            if (BubbleEntity)
            {
                BubbleEntity.ReturnToPool();
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
        }

        public void SetNeighbourList(List<GridData> neighbourList)
        {
            NeighbourGridDataList = neighbourList;
        }
    }
}