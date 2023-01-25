using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridDataController gridDataController;
        [SerializeField] private BubbleController bubbleController;
        
        private void Awake()
        {
            InitializeGame();
        }

        private void Start()
        {
            bubbleController.GenerateBubblesForStart();
            bubbleController.ActivateInitThrowBubbles();
        }

        private void InitializeGame()
        {
            gridDataController.Initialize();
            bubbleController.Initialize(gridDataController);
        }
    }
}
