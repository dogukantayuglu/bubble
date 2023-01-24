using Game.Scripts.Interfaces;
using Game.Scripts.Player;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public IBubbleBuffer BubbleBuffer
        {
            set => playerBubbleHandler.BubbleBuffer = value;
        }

        [SerializeField] private PlayerBubbleHandler playerBubbleHandler;
        [SerializeField] private AimHandler aimHandler;
        [SerializeField] private Transform playerCenterTransform;

        public void Initialize()
        {
            var playerCenterPosition = playerCenterTransform.position;
            playerBubbleHandler.Initialize(playerCenterPosition);
            aimHandler.Initialize(playerCenterPosition);
        }
        
        public void ActivateInitBubbles()
        {
            playerBubbleHandler.ActivateInitBubbles();
        }
    }
}