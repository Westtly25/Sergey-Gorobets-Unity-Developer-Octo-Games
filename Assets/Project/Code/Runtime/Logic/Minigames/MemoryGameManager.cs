using System;
using Naninovel;
using UnityEngine;
using Assets.Project.Code.Runtime.Logic.Minigames.UI;

namespace Assets.Project.Code.Runtime.Logic.Minigames
{
    [InitializeAtRuntime]
    public class MemoryGameManager : IMemoryGameManager
    {
        [SerializeField]
        private BoardView board;

        [SerializeField]
        private Timer timer;

        private bool isPaused;
        private bool isGameActive;
        private int amountOfTurns = 0;

        private MemoryGameSettings settings;

        public bool IsPaused => isPaused;
        public bool IsGameActive => isGameActive;
        public TimeSpan Time { get; }

        public event Action Started;
        public event Action<bool> Paused;
        public event Action<MemoryGameResults> Finish;

        private void OnEnable()
        {
            board.CardsTurned += IncreaseTurnAmount;
            board.AllCardsMatched += ForceFinish;
        }

        private void OnDisable()
        {
            board.CardsTurned -= IncreaseTurnAmount;
            board.AllCardsMatched -= ForceFinish;
        }

        public void StartGame()
        {
            //this.settings = settings;
            amountOfTurns = 0;
            isPaused = false;
            isGameActive = true;
            //timer.Begin();

            board.SetupGame(this.settings);
            Started?.Invoke();
        }

        public void Pause()
        {
            isPaused = true;
            //timer.Pause();
            Paused?.Invoke(isPaused);
        }

        public void Continue()
        {
            isPaused = false;
            //timer.Resume();
            Paused?.Invoke(isPaused);
        }

        public void Restart()
        {
            if (isPaused)
                Continue();

            StartGame();
        }

        public void Stop()
        {
            isGameActive = false;
            timer.Stop();
        }

        public void ForceFinish()
        {
            timer.Stop();
            isGameActive = false;
           // Finish?.Invoke(new MemoryGameResults(timer.TimePassed, amountOfTurns));
        }

        private void IncreaseTurnAmount() => amountOfTurns++;

        public async UniTask InitializeServiceAsync()
        {

            await UniTask.CompletedTask;
        }

        public void ResetService()
        {
        }

        public void DestroyService()
        {
        }
    }
}