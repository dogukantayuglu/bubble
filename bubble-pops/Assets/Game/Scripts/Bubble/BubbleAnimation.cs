using DG.Tweening;
using Game.Scripts.Data.Game;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleAnimation : MonoBehaviour
    {
        [Header("Activation")]
        [SerializeField] private float activationAnimationDuration = 0.5f;

        [Header("Fall")]
        [SerializeField] private float fallXPositionRandomAmount = 1f;
        [SerializeField] private float fallJumpRandomAmount = 5f;
        [SerializeField] private float dropDuration = 1f;

        [Header("Explode")]
        [SerializeField] private float outerCircleLocalScale = 1f;
        [SerializeField] private Transform outerCircleTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("DestroyByExplosion")] 
        [SerializeField] private float affectedAnimationDuration = 0.7f;
        [SerializeField] private float movementMultiplier = 1.4f;

        private float _explosionDuration = 0.8f;
        private Transform _transform;

        public void Initialize(float explosionDuration)
        {
            _transform = transform;
            _transform.localScale = Vector3.zero;
            _explosionDuration = explosionDuration;
        }

        public void ActivationAnimation(bool smallSize = false)
        {
            var size = smallSize ? GameData.BubbleSize * 0.7f : GameData.BubbleSize;
            _transform.DOScale(size, activationAnimationDuration);
        }

        public Tween DropAnimation()
        {
            var currentPosition = _transform.position;
            var randomXTarget = currentPosition.x +
                                Random.Range(-fallXPositionRandomAmount, fallXPositionRandomAmount);
            var randomJumpPower = Random.Range(0, fallJumpRandomAmount);
            var targetPosition = new Vector3(randomXTarget, -6, currentPosition.z);
            return _transform.DOJump(targetPosition, randomJumpPower, 1, dropDuration);
        }

        public Tween PlayExplodeAnimation()
        {
            spriteRenderer.DOFade(0f, _explosionDuration); 
            return outerCircleTransform.DOScale(Vector3.one * outerCircleLocalScale, _explosionDuration);
        }

        public Tween PlayAffectionFromExplosionAnimation(Vector3 explosionOrigin)
        {
            var currentPosition = _transform.position;
            var direction = (currentPosition - explosionOrigin).normalized;
            var targetPosition = currentPosition + (direction * movementMultiplier);
            _transform.DOMove(targetPosition, affectedAnimationDuration).SetEase(Ease.OutCirc);
            return _transform.DOScale(Vector3.zero, affectedAnimationDuration);
        }
    }
}