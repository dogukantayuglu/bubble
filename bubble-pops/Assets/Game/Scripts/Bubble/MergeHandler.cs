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
        
        public void Initialize(Action onMergeComplete)
        {
            _onMergeComplete = onMergeComplete;
            _bubbleEntitiesOnGrid = new List<BubbleEntity>();
        }

        public void AddActiveBubble(BubbleEntity bubbleEntity)
        {
            _bubbleEntitiesOnGrid.Add(bubbleEntity);
        }

        public void CheckMerge(BubbleEntity bubbleEntity)
        {
            AddActiveBubble(bubbleEntity);
            _onMergeComplete.Invoke();
        }
    }
}
