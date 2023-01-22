using Game.Scripts.Enums;

namespace Game.Scripts.Data.Grid
{
    public class GridData
    {
        public readonly CoordinateData CoordinateData;
        public GridOccupationStates OccupationState;

        public GridData(CoordinateData coordinateData)
        {
            CoordinateData = coordinateData;
        }
    }
}
