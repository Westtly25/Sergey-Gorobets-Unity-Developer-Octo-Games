using Naninovel;
using UnityEngine;
using DTT.MinigameMemory;

[CommandAlias("stopMemoryMG")]
public class StopMemoryMinimage : Command, Command.ILocalizable
{
    public StringParameter Name;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        MemoryGameManager memoryGameManager = Engine.GetService<MemoryGameManager>();
        memoryGameManager.Stop();

#if UNITY_EDITOR
        Debug.Log("Stop Memory Minimage Command");
#endif

        await UniTask.CompletedTask;
    }
}