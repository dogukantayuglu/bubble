using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Data.Grid
{
    public class GridData
    {
        public readonly CoordinateData CoordinateData;
        public Vector2 Position;
        public GridOccupationStates OccupationState;

        public GridData(CoordinateData coordinateData, Vector2 position, GridOccupationStates occupationState)
        {
            CoordinateData = coordinateData;
            Position = position;
            OccupationState = occupationState;
        }
    }
}
