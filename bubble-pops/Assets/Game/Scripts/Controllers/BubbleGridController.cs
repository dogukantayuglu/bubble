using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Grid;
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
    }
}
