using UnityEngine;

namespace Assets.Project.Code.Runtime.Logic.Minigames.Algorithms
{
    public enum ShuffleAlgorithms
    {
        [InspectorName("Modern Fisher-Yates Shuffle")]
        FISHER_YATES = 0,

        [InspectorName("Knuth's Shuffle")]
        KNUTHS_SHUFFLE = 1,

        [InspectorName("Random assigned values")]
        ASSIGNED_VALUE = 2
    }
}