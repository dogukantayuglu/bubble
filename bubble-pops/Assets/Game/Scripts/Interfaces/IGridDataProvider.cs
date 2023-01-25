using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IGridDataProvider
    {
        GridData GetFreeGridData();
        GridData GetClosestFreeGridData(Vector2 position);
    }
}