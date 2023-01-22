using Game.Scripts.Bubble;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField] private BubbleGridController bubbleGridController;
        [SerializeField] private BubblePool bubblePool;

        public void Initialize()
        {
            bubblePool.Initialize();
            bubbleGridController.Initialize();
        }
    }
}
