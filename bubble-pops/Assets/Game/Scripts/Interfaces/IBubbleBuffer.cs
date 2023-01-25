﻿using Game.Scripts.Bubble;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleBuffer
    {
        BubbleEntity GetBubbleForPlayer();
        void AddActiveBubble(BubbleEntity bubbleEntity);
    }
}