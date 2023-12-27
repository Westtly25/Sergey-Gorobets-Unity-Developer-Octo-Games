using Naninovel;
using UnityEngine;
using Assets.Project.Code.Runtime.Architecture.Quests_Service;

namespace Assets.Project.Code.Runtime.Logic.Quests.Configurations
{
    [EditInProjectSettings]
    public sealed class QuestsConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "QuestsData";

        [Tooltip("Configuration of the resource loader used with unlockable resources.")]
        public ResourceLoaderConfiguration Loader = new ResourceLoaderConfiguration { PathPrefix = DefaultPathPrefix };

        [SerializeField]
        public QuestsDataContainer questsDataContainer;
    }
}
