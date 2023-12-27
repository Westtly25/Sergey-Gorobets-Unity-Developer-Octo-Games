using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace DTT.MinigameMemory 
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cardContent;
        [SerializeField]
        private Button _cardButton;

        private Sprite _frontSprite;
        private Sprite _backSprite;

        public bool IsShowing => _isShowing;
        public bool canClick = true;
        private bool _isShowing = false;

        public event Action<Card> Clicked;
        public event Action<Card, Card> GoodMatch;
        public event Action<Card, Card> BadMatch;

        private void OnEnable() => _cardButton.onClick.AddListener(OnClick);
        private void OnDisable() => _cardButton.onClick.RemoveListener(OnClick);

        public void Init(Sprite backSprite)
        {
            _backSprite = backSprite;
            _cardButton.image.sprite = _backSprite;
        }
       
        public void SetFrontsprite(Sprite frontSprite) => 
            _frontSprite = frontSprite;
        
        public void FlipCard(float speed)
        {
            this.StartCoroutine(Flip(Quaternion.Euler(0, (_isShowing ? 0 : 180), 0), _isShowing ? _backSprite : _frontSprite, speed));
            _isShowing = !_isShowing;
        }

        public void CompairToCard(Card otherCard)
        {
            if (this._frontSprite == otherCard._frontSprite)
                GoodMatch?.Invoke(this, otherCard);
            else
                BadMatch?.Invoke(this, otherCard);
        }

        
        public void MoveToPosition(Vector3 target) => 
            _cardContent.transform.localPosition = target;
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
            Quaternion myRotation = _cardContent.transform.localRotation;

            for (float t = 0; t < 1; t += Time.deltaTime / time) 
            {
                _cardContent.transform.localRotation = Quaternion.Lerp(myRotation, targetRotation, t);

                if (t > 0.5f && _cardButton.image.sprite != sprite)
                {
                    _cardButton.image.sprite = sprite;
                    _cardButton.transform.localRotation = targetRotation;
                }

                yield return null;
            }

            _cardContent.transform.localRotation = targetRotation;
        }

        private IEnumerator FadeOutCard(float time)
        {
            _cardButton.enabled = false;

            for (float t = 0; t < 1; t += Time.deltaTime / time)
            {
                Color color = _cardButton.image.color;
                _cardButton.image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, t));

                yield return null;
            }
        }

        private IEnumerator FadeInCard(float time)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / time)
            {
                Color color = _cardButton.image.color;
                _cardButton.image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, 1f, t));

                yield return null;
            }

            _cardButton.enabled = true;
        }
    }
}