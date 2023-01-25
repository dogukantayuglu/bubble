using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class MergeHandler : MonoBehaviour
    {
        private List<BubbleEntity> _bubbleEntitiesOnGrid;
        private IBubblePool _bubblePool;
        
        public void Initialize(IBubblePool bubblePool)
        {
            _bubblePool = bubblePool;
            _bubbleEntitiesOnGrid = new List<BubbleEntity>();
        }

        public void CheckMerge(BubbleEntity bubbleEntity)
        {
            _bubbleEntitiesOnGrid.Add(bubbleEntity);
        }
    }
}
