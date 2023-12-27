using Naninovel;
using System;

namespace DTT.MinigameMemory
{
    public interface IMemoryGameManager : IEngineService
    {
        bool IsGameActive { get; }
        bool IsPaused { get; }
        TimeSpan Time { get; }

        event Action<MemoryGameResults> Finish;
        event Action<bool> Paused;
        event Action Started;

        void StartGame();
        void Restart();
        void Pause();
        void Continue();
        void Stop();
        void ForceFinish();
    }
}