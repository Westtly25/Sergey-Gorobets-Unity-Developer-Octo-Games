using Naninovel;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Naninovel.Command;

[CommandAlias("PlayMemoryMG")]
public class PlayMemoryMinimage : Command
{
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //throw new System.NotImplementedException();

        Debug.Log("PlayMemoryMinimage Command");

        await UniTask.CompletedTask;
    }
}
