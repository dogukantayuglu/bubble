using System.Collections.Generic;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Grid
{
    public struct GridInitializationData
    {
        public readonly int Row;
        public readonly int Column;
        public readonly Vector2 Position;
        public readonly GridOccupationStates GridOccupationState;

        public GridInitializationData(int row, int column, Vector2 position, GridOccupationStates gridOccupationState)
        {
            Row = row;
            Column = column;
            Position = position;
            GridOccupationState = gridOccupationState;
        }
    }
}