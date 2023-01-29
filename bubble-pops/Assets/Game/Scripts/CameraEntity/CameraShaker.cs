using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.CameraEntity
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private float magnitude;

        private float _elapsedTime;
        private Vector3 _initCamLocalPosition;
        private Transform _camTransform;
        private bool _isShaking;

        public void Initialize(Transform camTransform)
        {
            _camTransform = camTransform;
            _initCamLocalPosition = _camTransform.localPosition;
        }

        public void ShakeCamera()
        {
            _isShaking = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShakeCamera();
            }

            if (!_isShaking) return;

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= duration)
            {
                StopShaking();
            }

            if (_camTransform.localPosition != _initCamLocalPosition)
            {
                _camTransform.localPosition = _initCamLocalPosition;
                return;
            }

            var randomX = Random.Range(-1f, 1f) * magnitude;
            var randomY = Random.Range(-1f, 1f) * magnitude;

            var targetLocalPosition = _camTransform.localPosition;
            targetLocalPosition.x += randomX;
            targetLocalPosition.y += randomY;
            _camTransform.localPosition = targetLocalPosition;
        }

        private void StopShaking()
        {
            _isShaking = false;
            _elapsedTime = 0f;
            _camTransform.localPosition = _initCamLocalPosition;
        }
    }
}