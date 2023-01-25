using DG.Tweening;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class GhostBubbleEntity : MonoBehaviour
    {
        private enum GhostBubbleStates
        {
            Inactive,
            Activating,
            Active,
            Deactivating
        }

        [SerializeField] private float animationDuration = 0.5f;

        private Transform _transform;
        private GhostBubbleStates _currentState;
        private GridData _currentGridData;

        public void Initialize()
        {
            _currentState = GhostBubbleStates.Inactive;
            _transform = transform;
            _transform.localScale = Vector3.zero;
        }

        public void ActivateAtGrid(GridData gridData)
        {
            if (gridData.Equals(_currentGridData)) return;
            if (_currentState.Equals(GhostBubbleStates.Deactivating)) return;
            if (_currentState.Equals(GhostBubbleStates.Activating)) return;

            if (_currentState.Equals(GhostBubbleStates.Active))
            {
                ChangeForPositionChange(gridData);
                return;
            }

            _currentState = GhostBubbleStates.Activating;
            _currentGridData = gridData;
            _transform.position = gridData.Position;
            _transform.DOScale(Vector3.one * GameData.BubbleSize, animationDuration).OnComplete(SetStateActive);
        }

        private void ChangeForPositionChange(GridData gridData)
        {
            _currentState = GhostBubbleStates.Deactivating;
            _transform.DOScale(Vector3.zero, animationDuration).OnComplete(() => ChangePosition(gridData));
        }

        public void Deactivate()
        {
            _currentState = GhostBubbleStates.Deactivating;
            _currentGridData = null;
            _transform.DOScale(Vector3.zero, animationDuration).OnComplete(SetStateInactive);
        }

        private void SetStateActive()
        {
            _currentState = GhostBubbleStates.Active;
        }

        private void SetStateInactive()
        {
            _currentState = GhostBubbleStates.Inactive;
        }

        private void ChangePosition(GridData gridData)
        {
            _currentState = GhostBubbleStates.Inactive;
            ActivateAtGrid(gridData);
        }
    }
}