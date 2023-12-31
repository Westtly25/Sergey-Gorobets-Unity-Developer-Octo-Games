﻿using Naninovel;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Logic.Minigames.Commands
{
    [CommandAlias("restartMemoryMG")]
    public class RestartMemoryMinimage : Command, Command.ILocalizable
    {
        public StringParameter Name;

        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            MemoryGameManager memoryGameManager = Engine.GetService<MemoryGameManager>();
            memoryGameManager.Restart();

#if UNITY_EDITOR
            Debug.Log("Restart Memory Minimage Command");
#endif

            await UniTask.CompletedTask;
        }
    }
}