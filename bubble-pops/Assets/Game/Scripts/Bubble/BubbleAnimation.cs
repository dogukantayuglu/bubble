using DG.Tweening;
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
        }
        
        public void PlayActivationAnimation()
        {
            _transform.DOScale(Vector3.one, activationAnimationDuration);
        }
    }
}
