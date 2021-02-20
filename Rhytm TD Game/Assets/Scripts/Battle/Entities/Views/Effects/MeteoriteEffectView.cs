using RhytmTD.Data;
using UnityEngine;

namespace RhytmTD.Battle.Entities.Views.Effects
{
    public class MeteoriteEffectView : BattleEntityView
    {
        private bool m_Test = false;

        public override void Initialize(BattleEntity entity)
        {
            base.Initialize(entity);

            EffectModule effectModule = entity.GetModule<EffectModule>();
            effectModule.OnEffectAction += EffectActionHandler;
        }

        private void Update()
        {
            if (m_Test)
            {
                transform.position += Vector3.down * Time.deltaTime * 5;
            }
        }

        private void EffectActionHandler(DataContainer data)
        {
            string action = data.GetString("action");
            switch (action)
            {
                case "create":
                    {
                        /// will be moved to createEffect(position)
                        Vector3 position = data.GetVector("position");
                        transform.position = position + Vector3.up * 10;

                        m_Test = true;
                    }
                    break;
                case "blow":
                    UnityEngine.Debug.Log("BLOWWWW");
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
