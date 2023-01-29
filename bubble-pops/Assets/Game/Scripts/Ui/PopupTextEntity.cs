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
        private float anchoredStartPosition = -45f;
        [SerializeField] private float verticalMovementSpeed = 50f;
        [SerializeField] private float movementYTarget = 350f;
        [SerializeField] private float animationDuration = 1f;

        private Action<PopupTextEntity> _returnToPoolAction;

        public void Initialize(Action<PopupTextEntity> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;
            Reset();
        }

        private void Reset()
        {
            ResetPosition();
            gameObject.SetActive(false);
        }

        private void ResetPosition()
        {
            var position = rectTransform.anchoredPosition;
            position.x = 0f;
            position.y = anchoredStartPosition;
            rectTransform.anchoredPosition = position;
        }

        public void PlayTextAnimation(string text)
        {
            textMeshProUGUI.DOFade(1, 0);
            textMeshProUGUI.text = text;
            gameObject.SetActive(true);
            rectTransform.DOAnchorPosY(movementYTarget, verticalMovementSpeed).SetEase(Ease.OutCirc)
                .SetSpeedBased();
            textMeshProUGUI.DOFade(0, animationDuration).OnComplete(StopAnimation);
        }

        public void StopAnimation()
        {
            rectTransform.DOKill();
            Reset();
            _returnToPoolAction.Invoke(this);
        }
    }
}