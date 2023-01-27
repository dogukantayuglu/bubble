using System;
using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class MergeHandler : MonoBehaviour
    {
        private List<BubbleEntity> _bubbleEntitiesOnGrid;
        private Action _onMergeComplete;
        private IGridDataController _gridDataController;

        public void Initialize(IGridDataController gridDataController, Action onMergeComplete)
        {
            _onMergeComplete = onMergeComplete;
            _gridDataController = gridDataController;
            _bubbleEntitiesOnGrid = new List<BubbleEntity>();
        }

        public void AddActiveBubble(BubbleEntity bubbleEntity)
        {
            _bubbleEntitiesOnGrid.Add(bubbleEntity);
        }

        public void CheckMerge(BubbleEntity bubbleEntity)
        {
            var neighbourList = _gridDataController.GetNeighbourGridData(bubbleEntity.GridData);
            foreach (var neighbourGrid in neighbourList)
            {
                print($"{neighbourGrid.Row} {neighbourGrid.Column}");
            }
            // _onMergeComplete.Invoke();
        }
    }
}