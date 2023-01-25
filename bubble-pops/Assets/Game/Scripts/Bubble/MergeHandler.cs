using System;
using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class MergeHandler : MonoBehaviour
    {
        private List<BubbleEntity> _bubbleEntitiesOnGrid;
        private IBubblePool _bubblePool;
        private Action _onMergeComplete;
        
        public void Initialize(IBubblePool bubblePool, Action onMergeComplete)
        {
            _onMergeComplete = onMergeComplete;
            _bubblePool = bubblePool;
            _bubbleEntitiesOnGrid = new List<BubbleEntity>();
        }

        public void CheckMerge(BubbleEntity bubbleEntity)
        {
            _bubbleEntitiesOnGrid.Add(bubbleEntity);
            _onMergeComplete.Invoke();
        }
    }
}
