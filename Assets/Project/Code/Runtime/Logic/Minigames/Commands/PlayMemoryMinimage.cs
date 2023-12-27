using Naninovel;
using UnityEngine;
using DTT.MinigameMemory;

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