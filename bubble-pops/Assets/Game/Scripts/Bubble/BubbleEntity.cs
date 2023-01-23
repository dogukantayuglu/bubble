using Game.Scripts.Data.Bubble;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleEntity : MonoBehaviour
    {
        public int Value => _value;

        [SerializeField] private BubbleAnimation bubbleAnimation;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro valueText;

        private readonly BubbleCoordinateData _bubbleCoordinate = new BubbleCoordinateData();
        private int _value;


        public void Initialize()
        {
            transform.localScale = Vector3.zero;
            bubbleAnimation.Initialize();
        }

        public void Activate(BubbleActivationData bubbleActivationData)
        {
            _value = bubbleActivationData.BubbleValueData.value;
            spriteRenderer.color = bubbleActivationData.BubbleValueData.color;
            transform.position = bubbleActivationData.ActivationPosition;
            valueText.text = $"{bubbleActivationData.BubbleValueData.valueText}";
            _bubbleCoordinate.Row = bubbleActivationData.ActivationCoordinateData.Row;
            _bubbleCoordinate.Column = bubbleActivationData.ActivationCoordinateData.Column;
            gameObject.SetActive(true);
            bubbleAnimation.PlayActivationAnimation();
        }
    }
}
