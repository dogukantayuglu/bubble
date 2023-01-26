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

        private IGridDataController _gridDataController;
        private GhostBubbleEntity _ghostBubbleEntity;

        public void Initialize(IGridDataController gridDataController)
        {
            GenerateGhostBubble();
            _gridDataController = gridDataController;
        }

        private void GenerateGhostBubble()
        {
            _ghostBubbleEntity = Instantiate(ghostBubbleEntityPrefab, Vector3.zero, Quaternion.identity);
            _ghostBubbleEntity.Initialize();
        }
        
        public void ActivateGhostBubble(RaycastHit hit)
        {
            var closestGridData = _gridDataController.GetClosestFreeGridData(hit.point);
            _ghostBubbleEntity.ActivateAtGrid(closestGridData);
        }
        
        public void DeactivateGhostBubble()
        {
            _ghostBubbleEntity.Deactivate();
        }
    }
}
