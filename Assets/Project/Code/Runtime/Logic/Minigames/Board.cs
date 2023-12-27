using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DTT.MinigameMemory
{
    public class Board : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("GridLayoutGroup used to position the cards.")]
        private GridLayoutGroup _grid;

        [SerializeField]
        [Tooltip("Percentage of card size used for space between te cards.")]
        private float _cardSpacing;

        [SerializeField]
        private float _maximumCardSize = 0f;

        [SerializeField]
        [Tooltip("Prefab used to create the cards shown on the board.")]
        private GameObject _cardPrefab;

        private List<Sprite> _cardsInGame = new List<Sprite>();
        private List<Card> _cardsOnBoard = new List<Card>();
        private List<Card> _activeCards = new List<Card>();
        private List<Card> _inactiveCards = new List<Card>();
        private IShuffleAlgorithm _shuffleAlgorithm;
        private int _maxCardsInRow;
        private Card _firstSelectedCard;
        private int _activateAtMatchesFoundPecentage;
        private float _cardAnimationSpeed;

        public event Action CardsTurned;
        public event Action AllCardsMatched;

        private void OnEnable()
        {
            foreach (Card card in _cardsOnBoard)
            {
                card.Clicked += OnCardClicked;
                card.GoodMatch += RemoveMatchedCards;
                card.BadMatch += FlipMatchedCards;
            }
        }

        private void OnDisable()
        {
            foreach (Card card in _cardsOnBoard)
            {
                card.Clicked -= OnCardClicked;
                card.GoodMatch -= RemoveMatchedCards;
                card.BadMatch -= FlipMatchedCards;
            }
        }

        public void SetupGame(MemoryGameSettings settings)
        {
            _shuffleAlgorithm = settings.ShuffleAlgorithm;
            _activateAtMatchesFoundPecentage = settings.RefillAtFoundPercentage;
            _cardAnimationSpeed = settings.CardAnimationSpeed;

            ClearCards();

            CreateCards(settings.AmountOfCardsInGame, settings.CardSprites);

            SetupGrid(settings.AmountOfCardsOnBoard);

            CreateBoardCards(settings.AmountOfCardsOnBoard, settings.CardBacks);

            if (settings.AlignLastRow)
                AlignLastRowCards(settings.CardBacks);

            ActivateCards();
        }

        private void CreateCards(int amountOfCards, ReadOnlyCollection<Sprite> CardSprites)
        {
            int index;

            for (int i = 0; i < (amountOfCards / 2); i++)
            {
                index = Mathf.FloorToInt( Mathf.Repeat(i, CardSprites.Count));

                _cardsInGame.Add(CardSprites[index]);
                _cardsInGame.Add(CardSprites[index]);
            }

        }

        private void SetupGrid(int numberOfCards)
        {
            Rect gridRectangle= ((RectTransform)_grid.transform).rect;
            float availableWidth = gridRectangle.width;
            float availableHeight = gridRectangle.height;
            bool isLandscapeOrientation = availableWidth > availableHeight;

            int occupiedRows = isLandscapeOrientation ? 1 : numberOfCards;
            int occupiedColumns = isLandscapeOrientation ? numberOfCards : 1;

            while(
                (isLandscapeOrientation ? occupiedColumns > occupiedRows : occupiedRows > occupiedColumns) &&
                (isLandscapeOrientation ?
                    ((occupiedColumns % 2 == 0 && occupiedColumns / 2 != 1) || (occupiedColumns % 3 == 0 && occupiedColumns / 3 != 1)) :
                    ((occupiedRows % 2 == 0 && occupiedRows / 2 != 1) || (occupiedRows % 3 == 0 && occupiedRows / 3 != 1))
                    )
                )
            {
                if (isLandscapeOrientation)
                {
                    int division = occupiedColumns % 2 == 0 ? 2 : 3;
                    occupiedRows *= division;
                    occupiedColumns /= division;
                }
                else
                {
                    int division = occupiedRows % 2 == 0 ? 2 : 3;
                    occupiedRows /= division;
                    occupiedColumns *= division;
                }
            }

            _maxCardsInRow = Mathf.Max(occupiedRows, occupiedColumns);

            float widthSize = availableWidth / occupiedColumns;
            float heightSize = availableHeight / occupiedRows;
            widthSize -= widthSize * _cardSpacing;
            heightSize -= heightSize * _cardSpacing;
            float minimumSize = Mathf.Min(widthSize, heightSize);
            if (_maximumCardSize > 0f && minimumSize > _maximumCardSize)
                minimumSize = _maximumCardSize;

            _grid.cellSize = Vector2.one * minimumSize;
            _grid.spacing = Vector2.one * minimumSize * _cardSpacing;

            GridLayoutGroup.Constraint gridConstraint =
                isLandscapeOrientation ?
                GridLayoutGroup.Constraint.FixedColumnCount :
                GridLayoutGroup.Constraint.FixedRowCount;

            _grid.constraint = gridConstraint;
            _grid.constraintCount = _maxCardsInRow;
        }

        private void CreateBoardCards(int amountOfCardsOnBoard, ReadOnlyCollection<Sprite> backSprites)
        {
            int row;
            int cardbackIndex;
            
            _cardsOnBoard.Clear();
            _inactiveCards.Clear();

            for (int i = 0; i < amountOfCardsOnBoard; i++)
            {
                Card card = Instantiate(_cardPrefab, _grid.transform).GetComponent<Card>();

                row = Mathf.FloorToInt(i / _maxCardsInRow);
                cardbackIndex = ((i % _maxCardsInRow) + (row % backSprites.Count)) % backSprites.Count;

                card.Init(backSprites[cardbackIndex]);
                _cardsOnBoard.Add(card);
                _inactiveCards.Add(card);

                card.Clicked += OnCardClicked;
                card.GoodMatch += RemoveMatchedCards;
                card.BadMatch += FlipMatchedCards;
            }
        }

        private void ActivateCards()
        {
            List<Sprite> sprites = new List<Sprite>();
            if (_inactiveCards.Count < _cardsInGame.Count)
                sprites = _cardsInGame.GetRange(0, _inactiveCards.Count);
            else
                sprites = _cardsInGame;

            sprites = _shuffleAlgorithm.Shuffle(sprites);

            for (int i = 0; i < sprites.Count; i++)
            {
                Card card = _inactiveCards[i];
                card.SetFrontsprite(sprites[i]);
                _activeCards.Add(card);

                if (card.IsShowing)
                    card.FlipCard(_cardAnimationSpeed);

                card.EnableCard(_cardAnimationSpeed);
            }

            if (_inactiveCards.Count < _cardsInGame.Count)
                _cardsInGame.RemoveRange(0, _inactiveCards.Count);
            else
                _cardsInGame.Clear();
            
            _inactiveCards.Clear();
        }

        private void AlignLastRowCards(ReadOnlyCollection<Sprite> backSprites) 
        {
            int cardsInLastRow = _cardsOnBoard.Count % _maxCardsInRow;
            int row = Mathf.FloorToInt(_cardsOnBoard.Count / _maxCardsInRow);

            if (cardsInLastRow == _maxCardsInRow)
                return;

            int emptySlotsInLastRow = _maxCardsInRow - cardsInLastRow;
            float NumberOfCardWidths = emptySlotsInLastRow / 2f;
            float distance = (NumberOfCardWidths * _grid.cellSize.x) + (NumberOfCardWidths * _grid.spacing.x);
            int cardIndex;
            int cardbackIndex;

            for (int i = 0; i < cardsInLastRow; i++)
            {
                cardIndex = row * _maxCardsInRow + i;
                Vector3 newPosition = _cardsOnBoard[cardIndex].transform.localPosition + new Vector3(distance, 0, 0);
                _cardsOnBoard[cardIndex].MoveToPosition(new Vector3(distance, 0, 0));

                cardbackIndex = (i + (row + 1 % backSprites.Count)) % backSprites.Count;
                _cardsOnBoard[cardIndex].Init( backSprites[cardbackIndex]);
            }
        }

        private void ClearCards()
        {
            _cardsOnBoard.ForEach(card => Destroy(card.gameObject));
            foreach (Card card in _cardsOnBoard)
            {
                card.Clicked -= OnCardClicked;
                card.GoodMatch -= RemoveMatchedCards;
                card.BadMatch -= FlipMatchedCards;

                Destroy(card.gameObject);
            }

            _cardsInGame.Clear();
            _cardsOnBoard.Clear();
            _activeCards.Clear();
        }

        private void OnCardClicked(Card clickedCard)
        {
            clickedCard.FlipCard(_cardAnimationSpeed);

            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = clickedCard;
                _firstSelectedCard.canClick = false;
                return;
            }

            LockCards(true);

            StartCoroutine(CompareDelay(clickedCard));
        }

        private void RemoveMatchedCards(Card card1, Card card2)
        {
            _activeCards.Remove(card1);
            _inactiveCards.Add(card1);
            card1.DisableCard(_cardAnimationSpeed);

            _activeCards.Remove(card2);
            _inactiveCards.Add(card2);
            card2.DisableCard(_cardAnimationSpeed);

            if (_cardsInGame.Count == 0 && _activeCards.Count == 0)
                AllCardsMatched?.Invoke();
            if (_inactiveCards.Count >= Mathf.FloorToInt(_cardsOnBoard.Count * (_activateAtMatchesFoundPecentage / 100f)))
                ActivateCards();
            
            LockCards(false);
        }

        private void FlipMatchedCards(Card card1, Card card2)
        {
            card1.FlipCard(_cardAnimationSpeed);
            card2.FlipCard(_cardAnimationSpeed);

            LockCards(false);
        }

        private void LockCards(bool lockCards) => _activeCards.ForEach(card => card.canClick = !lockCards);

        private IEnumerator CompareDelay(Card clickedCard)
        {
            float delay = _cardAnimationSpeed;
            if (delay == 0)
                delay = 0.5f;

            yield return new WaitForSeconds(delay);

            CardsTurned?.Invoke();
            _firstSelectedCard.CompairToCard(clickedCard);
            _firstSelectedCard = null;
        }
    }
}