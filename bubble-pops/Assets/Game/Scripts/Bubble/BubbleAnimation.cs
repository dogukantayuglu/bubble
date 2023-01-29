using DG.Tweening;
using Game.Scripts.Data.Game;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleAnimation : MonoBehaviour
    {
        [SerializeField] private float activationAnimationDuration = 0.5f;

        [SerializeField] private float fallXPositionRandomAmount = 1f;
        [SerializeField] private float fallJumpRandomAmount = 5f;
        [SerializeField] private float dropDuration = 1f;

        private Transform _transform;

        public void Initialize()
        {
            _transform = transform;
            _transform.localScale = Vector3.zero;
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
    }
}