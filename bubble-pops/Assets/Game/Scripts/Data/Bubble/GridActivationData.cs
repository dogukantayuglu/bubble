using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public struct GridActivationData
    {
        public readonly GridData GridData;
        public readonly BubbleValueData BubbleValueData;

        public GridActivationData(GridData gridData, BubbleValueData bubbleValueData = null)
        {
            GridData = gridData;
            BubbleValueData = bubbleValueData;
        }
    }
}