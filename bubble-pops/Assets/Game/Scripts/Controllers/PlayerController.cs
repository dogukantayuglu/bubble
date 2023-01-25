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

        public void Initialize(BubbleController bubbleController, IGridDataProvider gridDataProvider)
        {
            var playerCenterPosition = playerCenterTransform.position;
            bubbleThrower.Initialize(bubbleController, gridDataProvider, playerCenterPosition);
            aimHandler.Initialize(bubbleThrower, playerCenterPosition);
        }

        public void ActivateInitBubbles()
        {
            bubbleThrower.ActivateInitBubbles();
        }
    }
}