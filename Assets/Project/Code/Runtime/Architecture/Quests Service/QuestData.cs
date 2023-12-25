using System;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    [Serializable]
    public sealed class QuestData
    {
        [SerializeField, TextArea(4, 10)]
        private string title;

        [SerializeField]
        private bool isCompleted = false;

        public string Title => title;
        public bool IsCompleted => isCompleted;
    }
}