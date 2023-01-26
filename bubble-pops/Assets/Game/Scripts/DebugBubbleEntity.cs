using DG.Tweening;
using Game.Scripts.Data.Grid;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class DebugBubbleEntity : MonoBehaviour
    {
        [SerializeField] private TextMeshPro coordinateText;

        public void SnapToGrid(GridData gridData)
        {
            SetCoordinate(gridData);
            transform.DOMove(gridData.Position, 1f);
        }
        private void SetCoordinate(GridData gridData)
        {
            coordinateText.text = $"{gridData.Row} {gridData.Column}";
        }
    }
}
