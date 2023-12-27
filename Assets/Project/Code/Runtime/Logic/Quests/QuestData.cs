using System;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Architecture.Quests_Service
{
    [Serializable]
    public sealed class QuestData
    {
        [SerializeField, Range(0, 9999)]
        private int id;
        [SerializeField, TextArea(4, 10)]
        private string title;
        [SerializeField]
        private QuestTask[] questTasks;
        [SerializeField]
        private bool isCompleted = false;

        public int Id => id;
        public string Title => title;
        public QuestTask[] QuestTasks => questTasks;
        public bool IsCompleted => isCompleted;
    }
}