using Naninovel;
using UnityEngine;
using DTT.MinigameMemory;

[CommandAlias("pauseMemoryMG")]
public class PauseMemoryMinimage : Command, Command.ILocalizable
{
    public StringParameter Name;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        MemoryGameManager memoryGameManager = Engine.GetService<MemoryGameManager>();
        memoryGameManager.Pause();

#if UNITY_EDITOR
        Debug.Log("Pause Memory Minimage Command");
#endif

        await UniTask.CompletedTask;
    }
}
