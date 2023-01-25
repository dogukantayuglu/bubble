using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridDataController gridDataController;
        [SerializeField] private BubbleController bubbleController;
        [SerializeField] private PlayerController playerController;
        
        private void Awake()
        {
            InitializeGame();
        }

        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            playerController.ActivateInitBubbles();
        }

        private void InitializeGame()
        {
            gridDataController.Initialize();
            bubbleController.Initialize(gridDataController);
            playerController.Initialize(bubbleController, gridDataController);
        }
    }
}
