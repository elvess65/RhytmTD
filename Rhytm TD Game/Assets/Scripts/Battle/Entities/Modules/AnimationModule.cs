using RhytmTD.Animation;
using RhytmTD.Battle.Entities.Views;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities
{
    /// <summary>
    /// Manages animation controller and is linked with view its init
    /// </summary>
    public class AnimationModule : IBattleModule
    {
        private AbstractAnimationView m_AnimationView;

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
    }
}
