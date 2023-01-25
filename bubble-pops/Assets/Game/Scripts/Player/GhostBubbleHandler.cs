using Game.Scripts.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class GhostBubbleHandler : MonoBehaviour, IBubbleAimTarget
    {
        public GridData TargetGridData => _ghostBubbleEntity.CurrentGridData;
        
        [SerializeField] private GhostBubbleEntity ghostBubbleEntityPrefab;

        private IGridBuffer _gridBuffer;
        private GhostBubbleEntity _ghostBubbleEntity;

        public void Initialize(IGridBuffer gridBuffer)
        {
            GenerateGhostBubble();
            _gridBuffer = gridBuffer;
        }

        private void GenerateGhostBubble()
        {
            _ghostBubbleEntity = Instantiate(ghostBubbleEntityPrefab, Vector3.zero, Quaternion.identity);
            _ghostBubbleEntity.Initialize();
        }
        
        public void ActivateGhostBubble(RaycastHit hit)
        {
            var closestGridData = _gridBuffer.GetClosesFreeGridData(hit.point);
            _ghostBubbleEntity.ActivateAtGrid(closestGridData);
        }
        
        public void DeactivateGhostBubble()
        {
            _ghostBubbleEntity.Deactivate();
        }
    }
}
