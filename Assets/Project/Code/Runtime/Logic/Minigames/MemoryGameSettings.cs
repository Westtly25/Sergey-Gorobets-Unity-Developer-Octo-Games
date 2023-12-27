using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.MinigameMemory
{
    /// <summary>
    /// Scriptable object storing the settings for a Memory game.
    /// </summary>
    [CreateAssetMenu(fileName = "Memory_GameSettings_Template", menuName = "DTT/MiniGame/Memory/GameSettings")]
    public class MemoryGameSettings : ScriptableObject
    {
        [Tooltip("Shuffle Algorithm")]
        public IShuffleAlgorithm ShuffleAlgorithm => GetAlgorithm();

        [Tooltip("CardView Mode")]
        public CardModes CardMode => _cardMode;

        [Tooltip("Mode to place cards on the board")]
        public BoardModes BoardMode => _boardMode;

        [Tooltip("If true aligns the last row of cards to the center")]
        public bool AlignLastRow => _alignLastRow;

        [Tooltip("Maximum amount of cards on the table")]
        public int AmountOfCardsInGame => GetAmountOfCardsInGame();

        [Tooltip("Maximum amount of cards on the table")]
        public int AmountOfCardsOnBoard => GetAmountOfCardsOnBoard();

        [Tooltip("Percentage of matches found untill refilling the board")]
        public int RefillAtFoundPercentage => _refillAtFoundPercentage;

        [Tooltip("Speed of the animations in seconds")]
        public float CardAnimationSpeed =>_cardAnimationSpeed;

        [Tooltip("Sprite used for the card backs")]
        public ReadOnlyCollection<Sprite> CardBacks => _cardBacks.AsReadOnly();

        [Tooltip("Sprites used for the card fronts")]
        public ReadOnlyCollection<Sprite> CardSprites => _cardSprites.AsReadOnly();

        // Algorithm Settings header.
        [Header("Algorithm Settings")]
        [SerializeField, Space(10)]
        [Tooltip("Determines the difficulty the game is played in.")]
        private ShuffleAlgorithms _shuffleAlgorithm;

        [SerializeField]
        [Tooltip("True: Alligns the bottom row of cards with the center.")]
        private bool _alignLastRow = true;

        [Header("Game Settings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Determines the use of card sprites.")]
        private CardModes _cardMode;

        [SerializeField]
        [Tooltip("Amount of cards the game is played with.")]
        private int _amountOfCards = 10;

        [Header("BoardView Sett ings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Determines the amount of cards placed on the board.")]
        private BoardModes _boardMode;

        [SerializeField]
        [Tooltip("Limit for the amount of cards on the board.")]
        private int _cardsOnBoardLimit = 4;

        [SerializeField]
        [Tooltip("Percentage of matches found untill refilling the board.")]
        [Range(0, 100)]
        private int _refillAtFoundPercentage = 100;

        [Header("CardView Settings")]
        [Space(10)]

        [SerializeField]
        [Tooltip("Speed of the animations in seconds.")]
        [Min(0f)]
        private float _cardAnimationSpeed = 1f;

        [SerializeField]
        [Tooltip("The sprite used on the back of the cards.")]
        private List<Sprite> _cardBacks;

        [SerializeField]
        [Tooltip("The sprites used to match cards with each other.")]
        private List<Sprite> _cardSprites;

        private void Reset()
        {
            _shuffleAlgorithm = ShuffleAlgorithms.FISHER_YATES;
            _cardMode = CardModes.USE_CARDS_ONCE;
            _boardMode = BoardModes.ALL_CARDS_ON_BOARD;
            _amountOfCards = 10;
            _cardsOnBoardLimit = 4;
            _refillAtFoundPercentage = 100;
            _cardAnimationSpeed = 1f;
            _alignLastRow = true;
            _cardBacks = new List<Sprite>();
            _cardSprites = new List<Sprite>();
        }

        private int GetAmountOfCardsInGame()
        {
            switch (_cardMode)
            {
                case CardModes.USE_CARDS_ONCE:
                    return _cardSprites.Count * 2;

                case CardModes.REUSE_CARDS:
                    if (_amountOfCards % 2 != 0)
                        _cardsOnBoardLimit--;

                    return _amountOfCards;

                default:
                    return _cardSprites.Count * 2;
            }
        }

        private int GetAmountOfCardsOnBoard()
        {
            int cardsInGame = GetAmountOfCardsInGame();
            int cardsOnBoard = 0;

            switch (_boardMode)
            {
                case BoardModes.ALL_CARDS_ON_BOARD:
                    cardsOnBoard = AmountOfCardsInGame;
                    break;

                case BoardModes.LIMIT_CARDS_ON_BOARD:
                    if (_cardsOnBoardLimit < 4)
                        _cardsOnBoardLimit = 4;

                    if (_cardsOnBoardLimit % 2 != 0)
                        _cardsOnBoardLimit--;

                    cardsOnBoard = _cardsOnBoardLimit;
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

            switch (_shuffleAlgorithm)
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