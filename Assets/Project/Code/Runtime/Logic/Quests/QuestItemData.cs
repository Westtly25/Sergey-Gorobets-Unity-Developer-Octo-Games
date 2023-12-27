using System;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    [Serializable]
    public sealed class QuestItemData
    {
        [SerializeField, TextArea(4, 10)]
        private string title;

        [SerializeField]
        private Sprite icon;

        public string Title => title;
        public Sprite Icon => icon;
    }
}