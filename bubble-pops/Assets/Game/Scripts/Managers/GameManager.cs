using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool GridDebugMode;
        
        [SerializeField] private GridController gridController;
        [SerializeField] private BubbleController bubbleController;
        [SerializeField] private bool gridDebugMode;
        
        private void Awake()
        {
            GridDebugMode = gridDebugMode;
            InitializeGame();
        }

        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            bubbleController.ActivateInitThrowBubbles();
        }

        private void InitializeGame()
        {
            gridController.Initialize();
            bubbleController.Initialize(gridController);
        }
    }
}
