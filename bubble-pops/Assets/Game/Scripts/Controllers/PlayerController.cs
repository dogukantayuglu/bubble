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

        public void Initialize()
        {
            playerBubbleHandler.Initialize();
        }
        
        public void ActivateInitBubbles()
        {
            playerBubbleHandler.ActivateInitBubbles();
        }
    }
}