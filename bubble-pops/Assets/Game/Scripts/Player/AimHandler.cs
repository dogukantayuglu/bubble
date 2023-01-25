using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class AimHandler : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private LineRenderer lineRenderer;

        private Vector3 _originPosition;
        private IBubbleTargetHandler _bubbleTargetHandler;
        private const string Reflector = "Reflector";

        public void Initialize(IBubbleTargetHandler bubbleTargetHandler, Vector3 originPosition)
        {
            _bubbleTargetHandler = bubbleTargetHandler;
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

            else
            {
                lineRenderer.enabled = false;
                _bubbleTargetHandler.DeactivateActiveGhostBubble();
            }
        }

        private void OnMouseButtonActive()
        {
            var direction = CalculateDirection();

            if (Mathf.Abs(direction.x) > 0.88f)
            {
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
            HandleBubbleHit(hit);
        }

        private void ReflectedHit(RaycastHit hit, Vector3 direction)
        {
            lineRenderer.SetPosition(1, hit.point);
            var reflectedDirection = direction;
            reflectedDirection.x *= -1;
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
            }

            else
            {
                lineRenderer.SetPosition(2, reflectedHit.point);
                HandleBubbleHit(reflectedHit);
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

        private void HandleBubbleHit(RaycastHit hit)
        {
            lineRenderer.enabled = true;
            _bubbleTargetHandler.HandleBubbleHit(hit);
        }
    }
}