using UnityEngine;

namespace DTT.MinigameMemory
{
    public enum BoardModes
    {
        [InspectorName("All cards on board.")]
        ALL_CARDS_ON_BOARD = 0,

        [InspectorName("Limited amount of cards on board")]
        LIMIT_CARDS_ON_BOARD = 1
    }
}