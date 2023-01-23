using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField] private BubbleGridController bubbleGridController;
        [SerializeField] private BubblePool bubblePool;
        [SerializeField] private BubbleValueSo bubbleValueSo;
        [SerializeField] private float massBubbleGenerationInterval = 0.1f;

        public void Initialize()
        {
            bubblePool.Initialize();
            bubbleGridController.Initialize();
            GenerateBubblesForStart();
        }
        
        private void GenerateBubblesForStart()
        {
            var gridDataList = GetInitRowsDataList();
            var sequence = DOTween.Sequence();

            var count = gridDataList.Count;
            for (var i = 0; i < count; i++)
            {
                var randomGridData = gridDataList[Random.Range(0, gridDataList.Count)];
                sequence.AppendCallback(()=> GenerateBubbleAtEmptyGrid(randomGridData));
                sequence.AppendInterval(massBubbleGenerationInterval);
                gridDataList.Remove(randomGridData);
            }
        }

        private List<GridData> GetInitRowsDataList()
        {
            var gridDataList = new List<GridData>();

            var gridData = bubbleGridController.GetFreeGridData();
            while (gridData.GridCoordinateData.Row <= 4)
            {
                gridDataList.Add(gridData);
                gridData = bubbleGridController.GetFreeGridData();
            }

            return gridDataList;
        }

        private void GenerateBubbleAtEmptyGrid(GridData gridData)
        {
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            
            var bubbleActivationData = new BubbleActivationData(
                gridData.GridCoordinateData, 
                gridData.Position,
                bubbleValueSo.GetSpawnableValue());
            
            bubbleEntity.Activate(bubbleActivationData);
        }
    }
}