using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubblePool : MonoBehaviour, IBubblePool
    {
        [SerializeField] private BubbleEntity bubbleEntityPrefab;
        [SerializeField] private int poolCount;

        private Stack<BubbleEntity> _bubblePool;

        public void Initialize()
        {
            _bubblePool = new Stack<BubbleEntity>();
            PoolBubbleEntities();
        }

        private void PoolBubbleEntities()
        {
            var poolTransform = transform;
            for (var i = 0; i < poolCount; i++)
            {
                var bubbleEntity = Instantiate(bubbleEntityPrefab, Vector3.zero, Quaternion.identity, poolTransform);
                bubbleEntity.Initialize();
                var bubbleGameObject = bubbleEntity.gameObject;
                bubbleGameObject.SetActive(false);
                bubbleGameObject.name = $"Bubble {i + 1}";
                _bubblePool.Push(bubbleEntity);
            }
        }

        public BubbleEntity GetBubbleFromPool()
        {
            return _bubblePool.Pop();
        }

        public void ReturnBubbleToPool(BubbleEntity bubbleEntity)
        {
            _bubblePool.Push(bubbleEntity);
        }
    }
}