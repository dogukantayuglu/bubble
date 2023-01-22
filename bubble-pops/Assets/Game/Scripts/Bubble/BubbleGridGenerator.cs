using System.Collections.Generic;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleGridGenerator : MonoBehaviour
    {
        [SerializeField] private int rowCount = 5;
        [SerializeField] private int columnCount = 5;
        [SerializeField] private float yStartPosition;
        [SerializeField] private float columnZigzagValue = 0.5f;
        

        public List<GridData> GenerateGrid()
        {
            var gridDataList = new List<GridData>();
            
            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                {
                    
                }
            }

            return gridDataList;
        }
    }
}
