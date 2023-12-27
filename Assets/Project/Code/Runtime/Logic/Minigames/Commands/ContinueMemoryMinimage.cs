using Naninovel;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Logic.Minigames.Commands
{
    [CommandAlias("continueMemoryMG")]
    public class ContinueMemoryMinimage : Command, Command.ILocalizable
    {
        public StringParameter Name;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            MemoryGameManager memoryGameManager = Engine.GetService<MemoryGameManager>();
            memoryGameManager.Continue();

#if UNITY_EDITOR
            Debug.Log("Continue Memory Minimage Command");
#endif

            await UniTask.CompletedTask;
        }
    }
}