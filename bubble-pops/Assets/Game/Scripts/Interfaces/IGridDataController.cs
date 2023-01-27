using System;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IGridDataController
    {
        GridData GetFreeGridData();
        GridData GetClosestFreeGridData(Vector2 position);
        void RecalculateGrid();
    }
}