using System;
using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubblePool : MonoBehaviour, IBubblePool
    {
        public List<BubbleEntity> ActiveBubbleEntities { get; private set; }
        [SerializeField] private BubbleEntity bubbleEntityPrefab;
        [SerializeField] private int poolCount;

        private Stack<BubbleEntity> _bubblePool;

        public void Initialize(Action<BubbleEntity> onMovementCompleteAction, float queueAnimationDuration)
        {
            ActiveBubbleEntities = new List<BubbleEntity>();
            _bubblePool = new Stack<BubbleEntity>();
            PoolBubbleEntities(onMovementCompleteAction, queueAnimationDuration);
        }

        private void PoolBubbleEntities(Action<BubbleEntity> onMovementCompleteAction, float queueAnimationDuration)
        {
            var poolTransform = transform;
            for (var i = 0; i < poolCount; i++)
            {
                var bubbleEntity = Instantiate(bubbleEntityPrefab, Vector3.zero, Quaternion.identity, poolTransform);
                bubbleEntity.Initialize(this, onMovementCompleteAction, queueAnimationDuration);
                var bubbleGameObject = bubbleEntity.gameObject;
                bubbleGameObject.SetActive(false);
                bubbleGameObject.name = $"Bubble {i + 1}";
                _bubblePool.Push(bubbleEntity);
            }
        }

        public BubbleEntity GetBubbleFromPool()
        {
            var bubbleEntity = _bubblePool.Pop();
            ActiveBubbleEntities.Add(bubbleEntity);
            return bubbleEntity;
        }

        public void ReturnBubbleToPool(BubbleEntity bubbleEntity)
        {
            bubbleEntity.gameObject.SetActive(false);
            bubbleEntity.transform.position = Vector3.zero;
            ActiveBubbleEntities.Remove(bubbleEntity);
            _bubblePool.Push(bubbleEntity);
        }
    }
}