using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridController gridController;
        [SerializeField] private BubbleController bubbleController;
        [SerializeField] private PlayerController playerController;
        
        private void Awake()
        {
            InitializeGame();
            SetInterfaces();
        }

        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            playerController.ActivateInitBubbles();
        }

        private void InitializeGame()
        {
            gridController.Initialize();
            bubbleController.Initialize();
            playerController.Initialize();
        }

        private void SetInterfaces()
        {
            bubbleController.GridBuffer = gridController;
            playerController.BubbleBuffer = bubbleController;
        }
    }
}
