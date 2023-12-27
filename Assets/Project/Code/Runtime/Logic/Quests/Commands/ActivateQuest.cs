using Naninovel;
using UnityEngine;
using Assets.Project.Code.Runtime.Logic.UI;
using Assets.Project.Code.Runtime.Architecture.Quests_Service;

[CommandAlias("activateQuest")]
public class ActivateQuest : Command
{
    [ParameterAlias("questName")]
    public StringParameter QuestName;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        IQuestManager questManager = Engine.GetService<IQuestManager>();
        QuestView questView = Engine.GetService<IUIManager>().GetUI<QuestView>();
        questManager.ActivateQuest(QuestName);

        if (!questView.Visible)
            await questView.ChangeVisibilityAsync(true);

        Debug.Log($"ActivateQuest : {QuestName}");
        await UniTask.CompletedTask;
    }
}
