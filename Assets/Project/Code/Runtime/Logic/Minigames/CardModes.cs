using UnityEngine;

namespace Assets.Project.Code.Runtime.Logic.Minigames
{
    public enum CardModes
    {
        [InspectorName("Use cards once.")]
        USE_CARDS_ONCE = 0,

        [InspectorName("Reuse cards.")]
        REUSE_CARDS = 1
    }
}