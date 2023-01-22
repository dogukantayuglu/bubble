using UnityEngine;

namespace Game.Scripts.Data.Grid
{
    public struct CoordinateData
    {
        public readonly int Row;
        public readonly int Column;

        public CoordinateData(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
