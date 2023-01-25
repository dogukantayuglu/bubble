using System.Collections.Generic;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class AimHandler : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private LineRenderer lineRenderer;

        private bool _isAiming;
        private Vector3 _reflectPoint;
        private Vector3 _originPosition;
        private IBubbleAimHandler _bubbleAimHandler;
        private IBubbleShooter _bubbleShooter;
        private const string Reflector = "Reflector";

        public void Initialize(IBubbleAimHandler bubbleAimHandler, IBubbleShooter bubbleShooter, Vector3 originPosition)
        {
            _bubbleShooter = bubbleShooter;
            _bubbleAimHandler = bubbleAimHandler;
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
                _bubbleShooter.ShootBubble(_reflectPoint);
                _bubbleAimHandler.DeactivateGhostBubble();
            }
        }

        private void OnMouseButtonActive()
        {
            _isAiming = true;
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
            _reflectPoint = Vector3.zero;
            HandleBubbleAimHit(hit);
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
            }

            else
            {
                lineRenderer.SetPosition(2, reflectedHit.point);
                HandleBubbleAimHit(reflectedHit);
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

        private void HandleBubbleAimHit(RaycastHit hit)
        {
            lineRenderer.enabled = true;
            _bubbleAimHandler.HandleBubbleAimHit(hit);
        }
    }
}