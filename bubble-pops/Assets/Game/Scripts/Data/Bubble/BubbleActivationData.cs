using Game.Scripts.Data.Grid;
using UnityEngine;

namespace Game.Scripts.Data.Bubble
{
    public struct BubbleActivationData
    {
        public readonly GridCoordinateData ActivationCoordinateData;
        public readonly Vector2 ActivationPosition;
        public readonly BubbleValueData BubbleValueData;

        public BubbleActivationData(GridCoordinateData activationCoordinateData, Vector2 activationPosition, BubbleValueData bubbleValueData)
        {
            ActivationCoordinateData = activationCoordinateData;
            ActivationPosition = activationPosition;
            BubbleValueData = bubbleValueData;
        }
    }
}
