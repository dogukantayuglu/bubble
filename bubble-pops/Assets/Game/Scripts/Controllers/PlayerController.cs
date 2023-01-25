using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerBubbleHandler playerBubbleHandler;
        [SerializeField] private AimHandler aimHandler;
        [SerializeField] private Transform playerCenterTransform;

        public void Initialize(BubbleController bubbleController)
        {
            var playerCenterPosition = playerCenterTransform.position;
            playerBubbleHandler.Initialize(bubbleController, playerCenterPosition);
            aimHandler.Initialize(bubbleController, playerBubbleHandler, playerCenterPosition);
        }

        public void ActivateInitBubbles()
        {
            playerBubbleHandler.ActivateInitBubbles();
        }
    }
}