using UnityEngine;
using RhytmTD.Animation;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class EnemyEntityAnimationView : AbstractAnimationView
    {
        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetKeyByType(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;

                case AnimationTypes.TakeDamage:
                    SetTrigger(key);
                    key = GetKeyByType(AnimationTypes.IdleBattle);
                    SetBool(key, true);
                    break;

                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Show:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Victory:
                    SetTrigger(key);
                    break;

                case AnimationTypes.IdleBattle:
                    SetBool(key, true);
                    break;

                case AnimationTypes.IdleNormal:
                    key = GetKeyByType(AnimationTypes.IdleBattle);
                    SetBool(key, false);
                    break;
            }
        }
    }
}

