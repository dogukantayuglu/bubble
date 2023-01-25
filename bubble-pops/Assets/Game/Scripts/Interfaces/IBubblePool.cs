using Game.Scripts.Bubble;

namespace Game.Scripts.Interfaces
{
    public interface IBubblePool
    {
        void ReturnBubbleToPool(BubbleEntity bubbleEntity);
    }
}
