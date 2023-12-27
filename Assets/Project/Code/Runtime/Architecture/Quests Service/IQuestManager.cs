using Assets.Project.Code.Runtime.Logic.Quests.Configurations;
using Naninovel;
using System;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    public interface IQuestManager : IEngineService<QuestsConfiguration>
    {
        event Action<QuestData> QuestActivated;
        event Action<QuestData> QuestCompleted;
        event Action<QuestData> QuestFailed;

        void ActivateQuest(string name);
        void ActivateQuest(int id);
    }
}