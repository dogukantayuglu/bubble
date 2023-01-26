using DG.Tweening;
using Game.Scripts.Data.Grid;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class DebugBubbleEntity : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private TextMeshPro coordinateText;

        public void SnapToGrid(GridData gridData)
        {
            gameObject.SetActive(isActive);
            SetCoordinate(gridData);
            transform.DOMove(gridData.Position, 0.3f);
        }
        private void SetCoordinate(GridData gridData)
        {
            coordinateText.text = $"{gridData.Row} {gridData.Column}";
        }

        public void GetDestroyed()
        {
            DestroyImmediate(gameObject);
        }
    }
}
