using Naninovel;
using UnityEngine;
using Naninovel.UI;
using Assets.Project.Code.Runtime.Architecture.Quests_Service;
using System.Collections.Generic;

namespace Assets.Project.Code.Runtime.Logic.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class QuestView : CustomUI, IManagedUI
    {
        [Tooltip("Container that will hold spawned choice buttons.")]
        [SerializeField]
        private RectTransform contentContainer;

        [Tooltip("Quest Slot prototype to use by default.")]
        [SerializeField]
        private QuestSlotView questSlotView;

        [Tooltip("Whether to focus added choice buttons for keyboard and gamepad control.")]
        [SerializeField]
        private bool focusChoiceButtons = true;

        [Tooltip("Active quests in content")]
        [SerializeField]
        private List<QuestSlotView> activeQuestSlots;

        [Header("Services")]
        private IQuestManager questManager;

        protected override void Awake()
        {
            base.Awake();
            questManager = Engine.GetService<IQuestManager>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            questManager.QuestActivated += OnQuestActivated;
            questManager.QuestCompleted += OnQuestCompleted;
        }

        private void OnQuestActivated(QuestData questData)
        {

        }

        private void OnQuestCompleted(QuestData questData)
        {

        }
    }
}