using Game.Scripts.Data.Bubble;
using Game.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro valueText;
        [SerializeField] private GameObject outerCircle;
        
        public void Initialize()
        {
            if (GameManager.GridDebugMode)
                valueText.gameObject.SetActive(false);
        }

        public void SetBubbleVisual(BubbleValueData bubbleValueData)
        {
            spriteRenderer.color = bubbleValueData.color;
            valueText.text = $"{bubbleValueData.valueText}";
            outerCircle.gameObject.SetActive(bubbleValueData.value > 512);
        }
    }
}
