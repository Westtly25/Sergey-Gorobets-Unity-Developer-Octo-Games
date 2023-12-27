using System;
using Naninovel;
using UnityEngine;
using Assets.Project.Code.Runtime.Logic.Quests.Configurations;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    [InitializeAtRuntime]
    public sealed class QuestManager : IQuestManager
    {
        public event Action<QuestData> QuestActivated;
        public event Action<QuestData> QuestCompleted;
        public event Action<QuestData> QuestFailed;

        private readonly IResourceProviderManager providerManager;

        public QuestsConfiguration Configuration { get; }

        public QuestManager(IResourceProviderManager providerManager)
        {
            this.providerManager = providerManager;
        }

        public async UniTask InitializeServiceAsync()
        {
            Debug.Log("QuestManager");

            await UniTask.CompletedTask;
        }

        public void ResetService()
        {

        }

        public void DestroyService()
        {

        }

        public void ActivateQuest(string name)
        {
            QuestActivated?.Invoke(null);
        }

        public void ActivateQuest(int id)
        {

        }
    }
}