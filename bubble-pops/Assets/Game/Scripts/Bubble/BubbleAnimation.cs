using DG.Tweening;
using Game.Scripts.Data.Game;
using UnityEngine;

namespace Game.Scripts.Bubble
{
    public class BubbleAnimation : MonoBehaviour
    {
        [SerializeField] private float activationAnimationDuration = 0.5f;
        private Transform _transform;

        public void Initialize()
        {
            _transform = transform;
            _transform.localScale = Vector3.zero;
        }
        
        public void PlayActivationAnimation(bool smallSize = false)
        {
            var size = smallSize ? GameData.BubbleSize * 0.7f : GameData.BubbleSize;
            _transform.DOScale(size, activationAnimationDuration);
        }
    }
}
