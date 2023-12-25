using Naninovel;
using UnityEngine;

[InitializeAtRuntime]
public class MinigameManager : IMinigameManager
{
    public MinigameConfiguration Configuration => throw new System.NotImplementedException();
    private GameObject container;

    public MinigameManager()
    {
        //container = Engine.CreateObject("Minigame");
    }

    public void DestroyService()
    {
    }

    public async UniTask InitializeServiceAsync()
    {
        Debug.Log("MinigameManager initialized");

        await UniTask.CompletedTask;
    }

    public void ResetService()
    {
    }
}
