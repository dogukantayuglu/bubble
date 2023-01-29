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
        [SerializeField] private float circleExpandSpeed = 20f;
        [SerializeField] private Transform outerCircleTransform;
        [SerializeField] private SpriteRenderer circleSpriteRenderer;
        [SerializeField] private SpriteRenderer outerCircleSpriteRenderer;

        [Header("DestroyByExplosion")] 
        [SerializeField] private float affectedAnimationDuration = 0.7f;
        [SerializeField] private float explosionMovementMagnitude = 1.4f;

        [Header("Bounce")] 
        [SerializeField] private float bounceMagnitude = 0.5f;
        [SerializeField] private float bounceDuration = 0.3f;
        [SerializeField] private Transform bubbleVisualTransform;

        [Header("Particle")] 
        [SerializeField] private ParticleSystem bubbleParticle;

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
            outerCircleTransform.DOScale(Vector3.one * outerCircleLocalScale, circleExpandSpeed).SetSpeedBased();

            outerCircleSpriteRenderer.DOFade(0f, _explosionDuration);
            return circleSpriteRenderer.DOFade(0f, _explosionDuration); 
        }

        public Tween PlayAffectionFromExplosionAnimation(Vector3 explosionOrigin)
        {
            var currentPosition = _transform.position;
            var direction = (currentPosition - explosionOrigin).normalized;
            var targetPosition = currentPosition + (direction * explosionMovementMagnitude);
            _transform.DOMove(targetPosition, affectedAnimationDuration).SetEase(Ease.OutCirc);
            return _transform.DOScale(Vector3.zero, affectedAnimationDuration);
        }

        public void PlayBubbleParticle()
        {
            bubbleParticle.transform.parent = null;
            bubbleParticle.Play();
            var duration = bubbleParticle.main.duration;
            DOVirtual.DelayedCall(duration, () => bubbleParticle.transform.parent = _transform);
        }

        public void PlayBounceAnimation(Vector3 originPosition)
        {
            var currentPosition = _transform.position;
            var direction = (currentPosition - originPosition).normalized;
            direction.z = 0f;
            direction *= bounceMagnitude;
            var halfBounceDuration = bounceDuration * 0.5f;
            bubbleVisualTransform.DOLocalMove(direction, halfBounceDuration).OnComplete(() =>
            {
                bubbleVisualTransform.DOLocalMove(Vector3.zero, halfBounceDuration);
            });
        }
    }
}