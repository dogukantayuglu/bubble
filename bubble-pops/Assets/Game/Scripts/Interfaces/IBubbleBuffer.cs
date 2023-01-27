using System.Collections.Generic;
using Game.Scripts.Bubble;
using Game.Scripts.Data.Grid;

namespace Game.Scripts.Interfaces
{
    public interface IBubbleBuffer
    {
        BubbleEntity GetBubbleForPlayer();
        void GenerateBubblesForNewGridData(List<GridData> gridDataList);
    }
}