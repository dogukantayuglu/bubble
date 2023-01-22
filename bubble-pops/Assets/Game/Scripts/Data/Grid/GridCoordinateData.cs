namespace Game.Scripts.Data.Grid
{
    public struct GridCoordinateData
    {
        public readonly int Row;
        public readonly int Column;

        public GridCoordinateData(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
