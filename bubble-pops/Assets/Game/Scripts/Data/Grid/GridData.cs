using System;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Grid
{
    public class GridData
    {
        public readonly GridCoordinateData GridCoordinateData;
        public Vector2 Position;
        public GridOccupationStates OccupationState;

        public GridData(GridCoordinateData gridCoordinateData, Vector2 position, GridOccupationStates occupationState)
        {
            GridCoordinateData = gridCoordinateData;
            Position = position;
            OccupationState = occupationState;
        }
    }
}
