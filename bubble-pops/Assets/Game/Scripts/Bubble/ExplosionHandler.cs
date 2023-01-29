using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class ExplosionHandler : MonoBehaviour
    {
        public float ExplosionDuration => explosionDuration;
        
        [SerializeField] private int explosionRowRange = 1;
        [SerializeField] private int explosionColumnRange = 3;
        [SerializeField] private float explosionDuration = 0.8f;
        
        private List<BubbleEntity> _activeBubbleEntities;
        private List<BubbleEntity> _bubblesToExplode;

        public void Initialize(List<BubbleEntity> activeBubbleEntityList)
        {
            _activeBubbleEntities = activeBubbleEntityList;
            _bubblesToExplode = new List<BubbleEntity>();
        }
        
        public void HandleBubbleExplosion(BubbleEntity explodedBubbleEntity)
        {
            var explosionRow = explodedBubbleEntity.GridData.Row;
            var explosionColumn = explodedBubbleEntity.GridData.Column;
            var explosionOrigin = explodedBubbleEntity.GridData.Position;

            foreach (var activeBubbleEntity in _activeBubbleEntities)
            {
                var bubbleRow = explodedBubbleEntity.GridData.Row;
                var bubbleColumn = explodedBubbleEntity.GridData.Column;
                var rowDistance = Mathf.Abs(explosionRow - bubbleRow);
                var columnDistance = Mathf.Abs(explosionColumn - bubbleColumn);
                if (rowDistance <= explosionRowRange && columnDistance <= explosionColumnRange)
                {
                    if (activeBubbleEntity == explodedBubbleEntity) continue;
                    _bubblesToExplode.Add(activeBubbleEntity);
                }
            }

            foreach (var entity in _bubblesToExplode)
            {
                entity.GetAffectedFromExplosion(explosionOrigin);
            }

            _bubblesToExplode.Clear();
        }
    }
}
