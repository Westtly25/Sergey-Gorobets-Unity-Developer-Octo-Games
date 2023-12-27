using Naninovel;
using UnityEngine;

[CommandAlias("playMemoryMG")]
public class PlayMemoryMinimage : Command, Command.ILocalizable
{
    public StringParameter Name;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {

        Debug.Log("PlayMemoryMinimage Command");

        await UniTask.CompletedTask;
    }
}