using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Naninovel.UI;

namespace DTT.MinigameMemory
{
    public class BoardView : CustomUI
    {
        [SerializeField]
        [Tooltip("GridLayoutGroup used to position the cards.")]
        private GridLayoutGroup grid;

        [SerializeField]
        [Tooltip("Percentage of card size used for space between te cards.")]
        private float cardSpacing;

        [SerializeField]
        private float maximumCardSize = 0f;

        [SerializeField]
        [Tooltip("Prefab used to create the cards shown on the board.")]
        private GameObject _cardPrefab;

        private IShuffleAlgorithm shuffleAlgorithm;
        private List<Sprite> cardsInGame = new List<Sprite>();
        private List<CardView> cardsOnBoard = new List<CardView>();
        private List<CardView> activeCards = new List<CardView>();
        private List<CardView> inactiveCards = new List<CardView>();
        private int maxCardsInRow;
        private CardView firstSelectedCard;
        private int activateAtMatchesFoundPecentage;
        private float cardAnimationSpeed;

        public event Action CardsTurned;
        public event Action AllCardsMatched;

        private void OnEnable()
        {
            foreach (CardView card in cardsOnBoard)
            {
                card.Clicked += OnCardClicked;
                card.GoodMatch += RemoveMatchedCards;
                card.BadMatch += FlipMatchedCards;
            }
        }

        private void OnDisable()
        {
            foreach (CardView card in cardsOnBoard)
            {
                card.Clicked -= OnCardClicked;
                card.GoodMatch -= RemoveMatchedCards;
                card.BadMatch -= FlipMatchedCards;
            }
        }

        public void SetupGame(MemoryGameSettings settings)
        {
            shuffleAlgorithm = settings.ShuffleAlgorithm;
            activateAtMatchesFoundPecentage = settings.RefillAtFoundPercentage;
            cardAnimationSpeed = settings.CardAnimationSpeed;

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

                cardsInGame.Add(CardSprites[index]);
                cardsInGame.Add(CardSprites[index]);
            }
        }

        private void SetupGrid(int numberOfCards)
        {
            Rect gridRectangle= ((RectTransform)grid.transform).rect;
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

            maxCardsInRow = Mathf.Max(occupiedRows, occupiedColumns);

            float widthSize = availableWidth / occupiedColumns;
            float heightSize = availableHeight / occupiedRows;
            widthSize -= widthSize * cardSpacing;
            heightSize -= heightSize * cardSpacing;
            float minimumSize = Mathf.Min(widthSize, heightSize);
            if (maximumCardSize > 0f && minimumSize > maximumCardSize)
                minimumSize = maximumCardSize;

            grid.cellSize = Vector2.one * minimumSize;
            grid.spacing = Vector2.one * minimumSize * cardSpacing;

            GridLayoutGroup.Constraint gridConstraint =
                isLandscapeOrientation ?
                GridLayoutGroup.Constraint.FixedColumnCount :
                GridLayoutGroup.Constraint.FixedRowCount;

            grid.constraint = gridConstraint;
            grid.constraintCount = maxCardsInRow;
        }

        private void CreateBoardCards(int amountOfCardsOnBoard, ReadOnlyCollection<Sprite> backSprites)
        {
            int row;
            int cardbackIndex;
            
            cardsOnBoard.Clear();
            inactiveCards.Clear();

            for (int i = 0; i < amountOfCardsOnBoard; i++)
            {
                CardView card = Instantiate(_cardPrefab, grid.transform).GetComponent<CardView>();

                row = Mathf.FloorToInt(i / maxCardsInRow);
                cardbackIndex = ((i % maxCardsInRow) + (row % backSprites.Count)) % backSprites.Count;

                card.Init(backSprites[cardbackIndex]);
                cardsOnBoard.Add(card);
                inactiveCards.Add(card);

                card.Clicked += OnCardClicked;
                card.GoodMatch += RemoveMatchedCards;
                card.BadMatch += FlipMatchedCards;
            }
        }

        private void ActivateCards()
        {
            List<Sprite> sprites = new List<Sprite>();
            if (inactiveCards.Count < cardsInGame.Count)
                sprites = cardsInGame.GetRange(0, inactiveCards.Count);
            else
                sprites = cardsInGame;

            sprites = shuffleAlgorithm.Shuffle(sprites);

            for (int i = 0; i < sprites.Count; i++)
            {
                CardView card = inactiveCards[i];
                card.SetFrontsprite(sprites[i]);
                activeCards.Add(card);

                if (card.IsShowing)
                    card.FlipCard(cardAnimationSpeed);

                card.EnableCard(cardAnimationSpeed);
            }

            if (inactiveCards.Count < cardsInGame.Count)
                cardsInGame.RemoveRange(0, inactiveCards.Count);
            else
                cardsInGame.Clear();
            
            inactiveCards.Clear();
        }

        private void AlignLastRowCards(ReadOnlyCollection<Sprite> backSprites) 
        {
            int cardsInLastRow = cardsOnBoard.Count % maxCardsInRow;
            int row = Mathf.FloorToInt(cardsOnBoard.Count / maxCardsInRow);

            if (cardsInLastRow == maxCardsInRow)
                return;

            int emptySlotsInLastRow = maxCardsInRow - cardsInLastRow;
            float NumberOfCardWidths = emptySlotsInLastRow / 2f;
            float distance = (NumberOfCardWidths * grid.cellSize.x) + (NumberOfCardWidths * grid.spacing.x);
            int cardIndex;
            int cardbackIndex;

            for (int i = 0; i < cardsInLastRow; i++)
            {
                cardIndex = row * maxCardsInRow + i;
                Vector3 newPosition = cardsOnBoard[cardIndex].transform.localPosition + new Vector3(distance, 0, 0);
                cardsOnBoard[cardIndex].MoveToPosition(new Vector3(distance, 0, 0));

                cardbackIndex = (i + (row + 1 % backSprites.Count)) % backSprites.Count;
                cardsOnBoard[cardIndex].Init( backSprites[cardbackIndex]);
            }
        }

        private void ClearCards()
        {
            cardsOnBoard.ForEach(card => Destroy(card.gameObject));
            foreach (CardView card in cardsOnBoard)
            {
                card.Clicked -= OnCardClicked;
                card.GoodMatch -= RemoveMatchedCards;
                card.BadMatch -= FlipMatchedCards;

                Destroy(card.gameObject);
            }

            cardsInGame.Clear();
            cardsOnBoard.Clear();
            activeCards.Clear();
        }

        private void OnCardClicked(CardView clickedCard)
        {
            clickedCard.FlipCard(cardAnimationSpeed);

            if (firstSelectedCard == null)
            {
                firstSelectedCard = clickedCard;
                firstSelectedCard.canClick = false;
                return;
            }

            LockCards(true);

            StartCoroutine(CompareDelay(clickedCard));
        }

        private void RemoveMatchedCards(CardView card1, CardView card2)
        {
            activeCards.Remove(card1);
            inactiveCards.Add(card1);
            card1.DisableCard(cardAnimationSpeed);

            activeCards.Remove(card2);
            inactiveCards.Add(card2);
            card2.DisableCard(cardAnimationSpeed);

            if (cardsInGame.Count == 0 && activeCards.Count == 0)
                AllCardsMatched?.Invoke();
            if (inactiveCards.Count >= Mathf.FloorToInt(cardsOnBoard.Count * (activateAtMatchesFoundPecentage / 100f)))
                ActivateCards();
            
            LockCards(false);
        }

        private void FlipMatchedCards(CardView card1, CardView card2)
        {
            card1.FlipCard(cardAnimationSpeed);
            card2.FlipCard(cardAnimationSpeed);

            LockCards(false);
        }

        private void LockCards(bool lockCards) => activeCards.ForEach(card => card.canClick = !lockCards);

        private IEnumerator CompareDelay(CardView clickedCard)
        {
            float delay = cardAnimationSpeed;
            if (delay == 0)
                delay = 0.5f;

            yield return new WaitForSeconds(delay);

            CardsTurned?.Invoke();
            firstSelectedCard.CompairToCard(clickedCard);
            firstSelectedCard = null;
        }
    }
}