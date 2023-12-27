using System.Collections.Generic;

namespace Assets.Project.Code.Runtime.Logic.Minigames.Algorithms
{
    public interface IShuffleAlgorithm
    {
        List<T> Shuffle<T>(List<T> list);
    }
}