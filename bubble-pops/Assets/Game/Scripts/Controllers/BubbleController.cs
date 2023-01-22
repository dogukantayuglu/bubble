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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateBubbleAtEmptyGridPosition();
            }
        }

        private void GenerateBubbleAtEmptyGridPosition()
        {
            var gridData = bubbleGridController.GetFreeGridData();
            if (gridData == null) return;
            var bubbleEntity = bubblePool.GetBubbleFromPool();
            bubbleEntity.transform.position = gridData.Position;
            bubbleEntity.gameObject.SetActive(true);
        }
    }
}