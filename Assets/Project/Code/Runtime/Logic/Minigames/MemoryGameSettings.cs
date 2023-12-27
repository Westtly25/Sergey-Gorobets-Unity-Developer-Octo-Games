using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Project.Code.Runtime.Logic.Minigames.Algorithms;

namespace Assets.Project.Code.Runtime.Logic.Minigames
{
    [CreateAssetMenu(fileName = "Memory_GameSettings_Template", menuName = "DTT/MiniGame/Memory/GameSettings")]
    public class MemoryGameSettings : ScriptableObject
    {
        [Header("Algorithm Settings")]
        [SerializeField, Space(10)]
        [Tooltip("Determines the difficulty the game is played in.")]
        private ShuffleAlgorithms shuffleAlgorithm;

        [SerializeField]
        [Tooltip("True: Alligns the bottom row of cards with the center.")]
        private bool alignLastRow = true;

        [Header("Game Settings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Determines the use of card sprites.")]
        private CardModes cardMode;

        [SerializeField]
        [Tooltip("Amount of cards the game is played with.")]
        private int amountOfCards = 10;

        [Header("BoardView Sett ings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Determines the amount of cards placed on the board.")]
        private BoardModes boardMode;

        [SerializeField]
        [Tooltip("Limit for the amount of cards on the board.")]
        private int cardsOnBoardLimit = 4;

        [SerializeField]
        [Tooltip("Percentage of matches found untill refilling the board.")]
        [Range(0, 100)]
        private int refillAtFoundPercentage = 100;

        [Header("CardView Settings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Speed of the animations in seconds.")]
        [Min(0f)]
        private float cardAnimationSpeed = 1f;

        [SerializeField]
        [Tooltip("The sprite used on the back of the cards.")]
        private List<Sprite> cardBacks;

        [SerializeField]
        [Tooltip("The sprites used to match cards with each other.")]
        private List<Sprite> cardSprites;

        [Tooltip("Shuffle Algorithm")]
        public IShuffleAlgorithm ShuffleAlgorithm => GetAlgorithm();

        [Tooltip("CardView Mode")]
        public CardModes CardMode => cardMode;

        [Tooltip("Mode to place cards on the board")]
        public BoardModes BoardMode => boardMode;

        [Tooltip("If true aligns the last row of cards to the center")]
        public bool AlignLastRow => alignLastRow;

        [Tooltip("Maximum amount of cards on the table")]
        public int AmountOfCardsInGame => GetAmountOfCardsInGame();

        [Tooltip("Maximum amount of cards on the table")]
        public int AmountOfCardsOnBoard => GetAmountOfCardsOnBoard();

        [Tooltip("Percentage of matches found untill refilling the board")]
        public int RefillAtFoundPercentage => refillAtFoundPercentage;

        [Tooltip("Speed of the animations in seconds")]
        public float CardAnimationSpeed =>cardAnimationSpeed;

        [Tooltip("Sprite used for the card backs")]
        public ReadOnlyCollection<Sprite> CardBacks => cardBacks.AsReadOnly();

        [Tooltip("Sprites used for the card fronts")]
        public ReadOnlyCollection<Sprite> CardSprites => cardSprites.AsReadOnly();


        private void Reset()
        {
            shuffleAlgorithm = ShuffleAlgorithms.FISHER_YATES;
            cardMode = CardModes.USE_CARDS_ONCE;
            boardMode = BoardModes.ALL_CARDS_ON_BOARD;
            amountOfCards = 10;
            cardsOnBoardLimit = 4;
            refillAtFoundPercentage = 100;
            cardAnimationSpeed = 1f;
            alignLastRow = true;
            cardBacks = new List<Sprite>();
            cardSprites = new List<Sprite>();
        }

        private int GetAmountOfCardsInGame()
        {
            switch (cardMode)
            {
                case CardModes.USE_CARDS_ONCE:
                    return cardSprites.Count * 2;

                case CardModes.REUSE_CARDS:
                    if (amountOfCards % 2 != 0)
                        cardsOnBoardLimit--;

                    return amountOfCards;

                default:
                    return cardSprites.Count * 2;
            }
        }

        private int GetAmountOfCardsOnBoard()
        {
            int cardsInGame = GetAmountOfCardsInGame();
            int cardsOnBoard = 0;

            switch (boardMode)
            {
                case BoardModes.ALL_CARDS_ON_BOARD:
                    cardsOnBoard = AmountOfCardsInGame;
                    break;

                case BoardModes.LIMIT_CARDS_ON_BOARD:
                    if (cardsOnBoardLimit < 4)
                        cardsOnBoardLimit = 4;

                    if (cardsOnBoardLimit % 2 != 0)
                        cardsOnBoardLimit--;

                    cardsOnBoard = cardsOnBoardLimit;
                    break;

                default:
                    cardsOnBoard = CardSprites.Count * 2;
                    break;
            }

            if (cardsOnBoard > cardsInGame)
                cardsOnBoard = cardsInGame;

            return cardsOnBoard;
        }

        private IShuffleAlgorithm GetAlgorithm()
        {
            IShuffleAlgorithm algorithm;

            switch (shuffleAlgorithm)
            {
                case ShuffleAlgorithms.FISHER_YATES:
                    algorithm = new FisherYatesShuffleAlgorithm();
                    break;

                case ShuffleAlgorithms.KNUTHS_SHUFFLE:
                    algorithm = new KnuthShuffleAlgorithm();
                    break;

                case ShuffleAlgorithms.ASSIGNED_VALUE:
                    algorithm = new AssignedValueShuffleAlgorithm();
                    break;

                default:
                    Debug.LogError("[DTT] - [Minigame - Memory] - [GameSettings] - " +
                        "ShuffleAlgorithm not recognised, using Fisher-Yates shuffle instead.");
                    algorithm = new FisherYatesShuffleAlgorithm();
                    break;
            }

            return algorithm;
        }
    }
}