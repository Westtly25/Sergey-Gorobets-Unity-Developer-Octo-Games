using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    [CreateAssetMenu(fileName = "Quests data container", menuName = "Novel / Quests / Create Quests Data Container", order = 1)]
    public sealed class QuestsDataContainer : ScriptableObject
    {
        [SerializeField]
        private List<QuestData> quests;

        public IEnumerable<QuestData> Quests => quests;
    }
}