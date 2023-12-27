using System;
using UnityEngine;
using Naninovel.UI;
using UnityEngine.UI;
using System.Collections;

namespace DTT.MinigameMemory 
{
    public class CardView : CustomUI
    {
        [SerializeField]
        private GameObject cardContent;
        [SerializeField]
        private Button cardButton;

        private Sprite frontSprite;
        private Sprite backSprite;

        public bool IsShowing => isShowing;
        public bool canClick = true;
        private bool isShowing = false;

        public event Action<CardView> Clicked;
        public event Action<CardView, CardView> GoodMatch;
        public event Action<CardView, CardView> BadMatch;

        protected override void OnEnable()
        {
            base.OnEnable();

            cardButton.onClick.AddListener(OnClick);
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            cardButton.onClick.RemoveListener(OnClick);
        }

        public void Init(Sprite backSprite)
        {
            this.backSprite = backSprite;
            cardButton.image.sprite = this.backSprite;
        }
       
        public void SetFrontsprite(Sprite frontSprite) => 
            this.frontSprite = frontSprite;
        
        public void FlipCard(float speed)
        {
            this.StartCoroutine(Flip(Quaternion.Euler(0, (isShowing ? 0 : 180), 0), isShowing ? backSprite : frontSprite, speed));
            isShowing = !isShowing;
        }

        public void CompairToCard(CardView otherCard)
        {
            if (this.frontSprite == otherCard.frontSprite)
                GoodMatch?.Invoke(this, otherCard);
            else
                BadMatch?.Invoke(this, otherCard);
        }

        public void MoveToPosition(Vector3 target) => 
            cardContent.transform.localPosition = target;

        public void DisableCard(float speed) => 
            this.StartCoroutine(FadeOutCard(speed));

        public void EnableCard(float speed) => 
            this.StartCoroutine(FadeInCard(speed));
        
        private void OnClick()
        {
            if (!canClick)
                return;

            Clicked?.Invoke(this);
        }

        private IEnumerator Flip(Quaternion targetRotation, Sprite sprite, float time)
        {
            Quaternion myRotation = cardContent.transform.localRotation;

            for (float t = 0; t < 1; t += Time.deltaTime / time) 
            {
                cardContent.transform.localRotation = Quaternion.Lerp(myRotation, targetRotation, t);

                if (t > 0.5f && cardButton.image.sprite != sprite)
                {
                    cardButton.image.sprite = sprite;
                    cardButton.transform.localRotation = targetRotation;
                }

                yield return null;
            }

            cardContent.transform.localRotation = targetRotation;
        }

        private IEnumerator FadeOutCard(float time)
        {
            cardButton.enabled = false;

            for (float t = 0; t < 1; t += Time.deltaTime / time)
            {
                Color color = cardButton.image.color;
                cardButton.image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, t));

                yield return null;
            }
        }

        private IEnumerator FadeInCard(float time)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / time)
            {
                Color color = cardButton.image.color;
                cardButton.image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, 1f, t));

                yield return null;
            }

            cardButton.enabled = true;
        }
    }
}