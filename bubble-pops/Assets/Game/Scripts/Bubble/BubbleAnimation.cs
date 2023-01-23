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
        
        public void PlayActivationAnimation()
        {
            _transform.DOScale(GameData.BubbleSize, activationAnimationDuration);
        }
    }
}
