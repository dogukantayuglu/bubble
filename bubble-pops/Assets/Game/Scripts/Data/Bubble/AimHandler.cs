using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public class AimHandler : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private LineRenderer lineRenderer;

        private bool _isAiming;
        private Vector3 _reflectPoint;
        private Vector3 _originPosition;
        private IBubbleThrower _bubbleThrower;
        private const string Reflector = "Reflector";
        private const float XDirectionLimit = 0.88f;

        public void Initialize(IBubbleThrower bubbleThrower, Vector3 originPosition)
        {
            _bubbleThrower = bubbleThrower;
            _originPosition = originPosition;
            lineRenderer.SetPosition(0, originPosition);
        }

        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButton(0))
            {
                OnMouseButtonActive();
            }

            else if (_isAiming)
            {
                _isAiming = false;
                lineRenderer.enabled = false;
                _bubbleThrower.ThrowBubble(_reflectPoint);
                _bubbleThrower.DeactivateGhostBubble();
            }
        }

        private void OnMouseButtonActive()
        {
            _isAiming = true;
            var direction = CalculateDirection();

            if (Mathf.Abs(direction.x) > XDirectionLimit)
            {
                _isAiming = false;
                lineRenderer.enabled = false;
                return;
            }

            if (Physics.Raycast(_originPosition, direction, out var hit))
            {
                HandleHit(hit, direction);
            }
        }

        private void HandleHit(RaycastHit hit, Vector3 direction)
        {
            if (hit.collider.CompareTag(Reflector))
            {
                ReflectedHit(hit, direction);
            }
            else
            {
                DirectHit(hit);
            }
        }

        private void DirectHit(RaycastHit hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.SetPosition(2, hit.point);
            _reflectPoint = Vector3.zero;
            HandleAimHit(hit);
        }

        private void ReflectedHit(RaycastHit hit, Vector3 direction)
        {
            lineRenderer.SetPosition(1, hit.point);
            var reflectedDirection = direction;
            reflectedDirection.x *= -1;
            _reflectPoint = hit.point;
            if (Physics.Raycast(hit.point, reflectedDirection, out var reflectedHit))
            {
                CheckSecondReflection(reflectedHit);
            }
        }

        private void CheckSecondReflection(RaycastHit reflectedHit)
        {
            if (reflectedHit.collider.CompareTag(Reflector))
            {
                lineRenderer.enabled = false;
                _bubbleThrower.DeactivateGhostBubble();
            }

            else
            {
                lineRenderer.SetPosition(2, reflectedHit.point);
                HandleAimHit(reflectedHit);
            }
        }

        private Vector3 CalculateDirection()
        {
            var inputPosition = Input.mousePosition;
            var position = gameCamera.ScreenToWorldPoint(inputPosition);
            position.z = 0f;
            var direction = (position - _originPosition).normalized;
            return direction;
        }

        private void HandleAimHit(RaycastHit hit)
        {
            lineRenderer.enabled = true;
            _bubbleThrower.ActivateGhostBubble(hit);
        }
    }
}