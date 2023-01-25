using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IGridBuffer
    {
        GridData GetFreeGridData();
        GridData GetClosesFreeGridData(Vector2 position);
    }
}