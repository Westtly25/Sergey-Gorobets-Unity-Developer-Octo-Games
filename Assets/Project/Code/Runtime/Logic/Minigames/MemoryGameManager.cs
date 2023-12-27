using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DTT.MinigameBase;
using DTT.MinigameBase.Timer;
using DTT.MinigameBase.UI;

namespace DTT.MinigameMemory
{
    public class MemoryGameManager : MonoBehaviour, IMinigame<MemoryGameSettings, MemoryGameResults>
    {  
        public bool IsPaused => _isPaused;
        public bool IsGameActive => _isGameActive;
        public TimeSpan Time => _timer.TimePassed;

        [SerializeField]
        private Board _board;

        [SerializeField]
        private Timer _timer;

        private bool _isPaused;
        private bool _isGameActive;

        private MemoryGameSettings _settings;

        private int _amountOfTurns = 0;

        public event Action Started;
        public event Action<bool> Paused;
        public event Action<MemoryGameResults> Finish;

        private void Awake() => _timer = (_timer == null) ? this.gameObject.AddComponent<Timer>() : _timer;

        private void OnEnable()
        {
            _board.CardsTurned += IncreaseTurnAmount;
            _board.AllCardsMatched += ForceFinish;
        }

        private void OnDisable()
        {
            _board.CardsTurned -= IncreaseTurnAmount;
            _board.AllCardsMatched -= ForceFinish;
        }

        public void StartGame(MemoryGameSettings settings)
        {
            _settings = settings;
            _amountOfTurns = 0;
            _isPaused = false;
            _isGameActive = true;
            _timer.Begin();

            _board.SetupGame(_settings);
            Started?.Invoke();
        }

        public void Pause()
        {
            _isPaused = true;
            _timer.Pause();
            Paused?.Invoke(_isPaused);
        }

        public void Continue()
        {
            _isPaused = false;
            _timer.Resume();
            Paused?.Invoke(_isPaused);
        }

        
        public void Restart()
        {
            if (_isPaused)
                Continue();

            StartGame(_settings);
        }

        public void Stop()
        {
            _isGameActive = false;
            _timer.Stop();
        }
       
        public void ForceFinish()
        {
            _timer.Stop();
            _isGameActive = false;
            Finish?.Invoke(new MemoryGameResults(_timer.TimePassed, _amountOfTurns));
        }

        private void IncreaseTurnAmount() => _amountOfTurns++;
    }
}