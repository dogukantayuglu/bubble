using DG.Tweening;
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
        [SerializeField] private ParticleSystem bubbleParticle;

        private Vector3 _initOuterCircleScale;
        
        public void Initialize()
        {
            if (GameManager.GridDebugMode)
                valueText.gameObject.SetActive(false);

            _initOuterCircleScale = outerCircle.transform.localScale;
        }

        public void SetBubbleVisual(BubbleValueData bubbleValueData)
        {
            outerCircle.transform.localScale = _initOuterCircleScale;
            spriteRenderer.color = bubbleValueData.color;
            SetBubbleParticleColor(bubbleValueData.color);
            valueText.text = $"{bubbleValueData.valueText}";
            valueText.DOFade(1, 0.1f);
            outerCircle.gameObject.SetActive(bubbleValueData.value > 512);
        }
        
        public void FadeOut(float duration)
        {
            spriteRenderer.DOFade(0.3f, duration);
            valueText.DOFade(0, duration - (duration * 0.5f));
        }

        private void SetBubbleParticleColor(Color color)
        {
            var main = bubbleParticle.main;
            main.startColor = color;
        }
    }
}
