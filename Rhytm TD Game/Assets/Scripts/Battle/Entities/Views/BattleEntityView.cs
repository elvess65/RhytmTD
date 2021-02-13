using CoreFramework;

namespace RhytmTD.Battle.Entities.Views
{
    public class BattleEntityView : BaseView
    {
        protected BattleEntity BattleEntity => m_BattleEntity;

        private BattleEntity m_BattleEntity;

        public virtual void Initialize(BattleEntity entity)
        {
            m_BattleEntity = entity;
        }
    }
}
