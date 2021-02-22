using RhytmTD.Animation;
using RhytmTD.Battle.Entities.Views;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Manages animation controller and is linked with view that holds reference to controller in world
    /// Transmits posibility to play animation from entity to view
    /// </summary>
    public class AnimationModule : IBattleModule
    {
        private AbstractAnimationView m_AnimationView;

        /// <summary>
        /// Manages animation controller and is linked with view that holds reference to controller in world
        /// Transmits posibility to play animation from entity to view
        /// </summary>
        public AnimationModule()
        {
        }

        public void InitializeModule(BattleEntityView entityView)
        {
            m_AnimationView = entityView.transform.GetComponent<AbstractAnimationView>();

            if (m_AnimationView != null)
                m_AnimationView.Initialize();
        }

        public void PlayAnimation(AnimationTypes animationType)
        {
            m_AnimationView.PlayAnimation(animationType);
        }

        public void PlayAttackAnimation()
        {
            PlayAnimation(AnimationTypes.Attack);
        }

        public void PlayIdleBattleAnimation()
        {
            PlayAnimation(AnimationTypes.IdleBattle);
        }
    }
}
