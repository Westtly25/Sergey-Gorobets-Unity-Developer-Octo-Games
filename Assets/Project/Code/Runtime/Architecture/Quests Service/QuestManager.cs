using Naninovel;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    public sealed class QuestManager : IEngineService
    {
        public async UniTask InitializeServiceAsync()
        {
            await UniTask.CompletedTask;
        }

        public void ResetService()
        {

        }

        public void DestroyService()
        {

        }
    }
}