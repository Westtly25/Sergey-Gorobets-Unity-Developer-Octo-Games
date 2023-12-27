using Naninovel;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Logic.Minigames.Commands
{
    [CommandAlias("playMemoryMG")]
    public class PlayMemoryMinimage : Command, Command.ILocalizable
    {
        public StringParameter Name;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            MemoryGameManager memoryGameManager = Engine.GetService<MemoryGameManager>();
            memoryGameManager.StartGame();

#if UNITY_EDITOR
            Debug.Log("Play Memory Minimage Command");
#endif

            await UniTask.CompletedTask;
        }
    }
}