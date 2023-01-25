using Game.Scripts.Interfaces;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BubbleThrower bubbleThrower;
        [SerializeField] private AimHandler aimHandler;
        [SerializeField] private Transform playerCenterTransform;

        public void Initialize(BubbleController bubbleController, IGridBuffer gridBuffer)
        {
            var playerCenterPosition = playerCenterTransform.position;
            bubbleThrower.Initialize(bubbleController, gridBuffer, playerCenterPosition);
            aimHandler.Initialize(bubbleThrower.BubbleAimTarget, bubbleThrower, playerCenterPosition);
        }

        public void ActivateInitBubbles()
        {
            bubbleThrower.ActivateInitBubbles();
        }
    }
}