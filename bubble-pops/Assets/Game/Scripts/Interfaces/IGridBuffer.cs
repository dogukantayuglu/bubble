using Game.Scripts.Data.Grid;

namespace Game.Scripts.Interfaces
{
    public interface IGridBuffer
    {
        GridData GetFreeGridData();
    }
}