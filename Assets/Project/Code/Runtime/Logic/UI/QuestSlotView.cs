using TMPro;
using UnityEngine;
using Naninovel.UI;

namespace Assets.Project.Code.Runtime.Logic.UI
{
    public sealed class QuestSlotView : CustomUI
    {
        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TaskSlotView taskSlotView;

        [SerializeField]
        [Tooltip("Container that will hold spawned choice buttons.")]
        private RectTransform contentContainer;

        public void Initialize()
        {

        }
    }
}