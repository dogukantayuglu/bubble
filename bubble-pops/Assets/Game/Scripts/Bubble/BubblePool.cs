using System;
using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubblePool : MonoBehaviour, IBubblePool
    {
        public Action<BubbleEntity> OnBubbleReturnedPool;
        
        [SerializeField] private BubbleEntity bubbleEntityPrefab;
        [SerializeField] private int poolCount;

        private Stack<BubbleEntity> _bubblePool;

        public void Initialize(Action<BubbleEntity> onBubblePlacedToGrid, float queueAnimationDuration)
        {
            _bubblePool = new Stack<BubbleEntity>();
            PoolBubbleEntities(onBubblePlacedToGrid, queueAnimationDuration);
        }

        private void PoolBubbleEntities(Action<BubbleEntity> onBubblePlacedToGrid, float queueAnimationDuration)
        {
            var poolTransform = transform;
            for (var i = 0; i < poolCount; i++)
            {
                var bubbleEntity = Instantiate(bubbleEntityPrefab, Vector3.zero, Quaternion.identity, poolTransform);
                bubbleEntity.Initialize(this, onBubblePlacedToGrid, queueAnimationDuration);
                var bubbleGameObject = bubbleEntity.gameObject;
                bubbleGameObject.SetActive(false);
                bubbleGameObject.name = $"Bubble {i + 1}";
                _bubblePool.Push(bubbleEntity);
            }
        }

        public BubbleEntity GetBubbleFromPool()
        {
            var bubbleEntity = _bubblePool.Pop();
            return bubbleEntity;
        }

        public void ReturnBubbleToPool(BubbleEntity bubbleEntity)
        {
            OnBubbleReturnedPool.Invoke(bubbleEntity);
            bubbleEntity.gameObject.SetActive(false);
            bubbleEntity.transform.position = Vector3.zero;
            _bubblePool.Push(bubbleEntity);
        }
    }
}