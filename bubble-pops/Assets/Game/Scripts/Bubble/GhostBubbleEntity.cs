using DG.Tweening;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class GhostBubbleEntity : MonoBehaviour
    {
        public GridData CurrentGridData => _currentGridData;
        private enum GhostBubbleStates
        {
            Inactive,
            Activating,
            Active,
            Deactivating
        }

        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Range(0.1f, 1f)]
        [SerializeField] private float opacity = 0.3f;

        private Transform _transform;
        private GhostBubbleStates _currentState;
        private GridData _currentGridData;
        private Color _targetColor;

        public void Initialize()
        {
            _currentState = GhostBubbleStates.Inactive;
            _transform = transform;
            _transform.localScale = Vector3.zero;
        }

        public void ActivateAtGrid(GridData gridData, Color color)
        {
            if (gridData.Equals(_currentGridData)) return;
            if (_currentState.Equals(GhostBubbleStates.Deactivating)) return;
            if (_currentState.Equals(GhostBubbleStates.Activating)) return;

            SetTargetColor(color);
            if (_currentState.Equals(GhostBubbleStates.Active))
            {
                DeactivateForPositionChange(gridData);
                return;
            }

            SetColor();
            _currentState = GhostBubbleStates.Activating;
            _currentGridData = gridData;
            var targetPosition = (Vector3)gridData.Position;
            targetPosition.z = 0.0001f;
            _transform.position = targetPosition;
            _transform.DOScale(Vector3.one * GameData.BubbleSize, animationDuration).OnComplete(SetStateActive);
        }

        private void SetColor()
        {
            spriteRenderer.color = _targetColor;
        }

        private void SetTargetColor(Color color)
        {
            color.a = opacity;
            _targetColor = color;
        }

        private void DeactivateForPositionChange(GridData gridData)
        {
            _currentState = GhostBubbleStates.Deactivating;
            _transform.DOScale(Vector3.zero, animationDuration).OnComplete(() => ChangePosition(gridData));
        }

        public void Deactivate()
        {
            _currentState = GhostBubbleStates.Deactivating;
            _transform.DOKill();
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
            ActivateAtGrid(gridData, _targetColor);
        }
    }
}