using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Enums;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BubbleGridController : MonoBehaviour
    {
        [SerializeField] private BubbleGridGenerator bubbleGridGenerator;

        private List<GridData> _gridDataList;

        public void Initialize()
        {
            _gridDataList = bubbleGridGenerator.GenerateGrid();
        }

        public GridData GetFreeGridData()
        {
            foreach (var gridData in _gridDataList)
            {
                if (gridData.OccupationState == GridOccupationStates.Free)
                {
                    gridData.OccupationState = GridOccupationStates.Occupied;
                    return gridData;
                }
            }

            return null;
        } 
    }
}
