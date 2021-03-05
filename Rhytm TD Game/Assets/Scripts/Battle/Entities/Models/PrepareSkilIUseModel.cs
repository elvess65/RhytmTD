using CoreFramework;

namespace RhytmTD.Battle.Entities.Models
{
    public class PrepareSkilIUseModel : BaseModel
    {
        public System.Action OnCorrectTouch;
        public System.Action OnWrongTouch;

        /// <summary>
        /// The skill sequence was reset (missed time or wrong input)
        /// </summary>
        public System.Action<int> OnSkillReset;

        /// <summary>
        /// All skill's sequences were failed. Break sequence
        /// </summary>
        public System.Action OnSequenceFailed;

        /// <summary>
        /// Skill step was reached using input
        /// </summary>
        public System.Action<int> OnSkillStepReachedInput;

        /// <summary>
        /// Skill step was reached using time
        /// </summary>
        public System.Action<int> OnSkillStepReachedAuto;

        /// <summary>
        /// Skill was selected
        /// </summary>
        public System.Action<int> OnSkillSelected;
    }
}
