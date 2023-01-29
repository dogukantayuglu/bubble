using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public class PopupTextEntity : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        [Header("Animation Values")] [SerializeField]
        private float targetScale = 2f;

        [SerializeField] private float animationDuration = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float fadeOutDurationPercentage = 0.4f;

        private Action<PopupTextEntity> _returnToPoolAction;

        public void Initialize(Action<PopupTextEntity> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;
            gameObject.SetActive(false);
        }


        public void PlayTextAnimation(string text)
        {
            textMeshProUGUI.DOFade(1, 0);
            textMeshProUGUI.text = text;
            gameObject.SetActive(true);
            rectTransform.DOScale(targetScale, animationDuration);
            DOVirtual.DelayedCall(animationDuration * (1 - fadeOutDurationPercentage), FadeOut);
        }

        private void FadeOut()
        {
            textMeshProUGUI.DOFade(0, animationDuration * fadeOutDurationPercentage).OnComplete(StopAnimation);
        }

        public void StopAnimation()
        {
            rectTransform.DOKill();
            gameObject.SetActive(false);
            rectTransform.localScale = Vector3.one;
            _returnToPoolAction.Invoke(this);
        }
    }
}