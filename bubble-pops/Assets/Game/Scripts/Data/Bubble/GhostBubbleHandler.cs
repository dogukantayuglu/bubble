using Game.Scripts.Bubble;
using Game.Scripts.Data.Grid;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public class GhostBubbleHandler : MonoBehaviour
    {
        public GridData TargetGridData => _ghostBubbleEntity.CurrentGridData;
        
        [SerializeField] private GhostBubbleEntity ghostBubbleEntityPrefab;

        private IGridDataProvider _gridDataProvider;
        private GhostBubbleEntity _ghostBubbleEntity;

        public void Initialize(IGridDataProvider gridDataProvider)
        {
            GenerateGhostBubble();
            _gridDataProvider = gridDataProvider;
        }

        private void GenerateGhostBubble()
        {
            _ghostBubbleEntity = Instantiate(ghostBubbleEntityPrefab, Vector3.zero, Quaternion.identity);
            _ghostBubbleEntity.Initialize();
        }
        
        public void ActivateGhostBubble(RaycastHit hit)
        {
            var closestGridData = _gridDataProvider.GetClosestFreeGridData(hit.point);
            _ghostBubbleEntity.ActivateAtGrid(closestGridData);
        }
        
        public void DeactivateGhostBubble()
        {
            _ghostBubbleEntity.Deactivate();
        }
    }
}
