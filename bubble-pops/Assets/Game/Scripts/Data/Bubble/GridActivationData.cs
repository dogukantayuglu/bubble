using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public struct GridActivationData
    {
        public readonly GridCoordinateData ActivationCoordinateData;
        public readonly Vector2 ActivationPosition;
        public readonly BubbleValueData BubbleValueData;

        public GridActivationData(GridCoordinateData activationCoordinateData, Vector2 activationPosition, BubbleValueData bubbleValueData)
        {
            ActivationCoordinateData = activationCoordinateData;
            ActivationPosition = activationPosition;
            BubbleValueData = bubbleValueData;
        }
    }
}
