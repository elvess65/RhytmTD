using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Animation
{
    public class DefaultBattleEntityAnimationController : AbstractAnimationController
    {
        //private int m_IdleHash;

        public override void Initialize()
        {
            base.Initialize();

            //Hash idle animation
            //m_IdleHash = Animator.StringToHash($"{m_BASE_LAYER}.{GetKeyByType(AnimationTypes.Idle)}");
        }

        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetKeyByType(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;

                case AnimationTypes.StartMove:
                    SetBool(key, true);
                    break;

                case AnimationTypes.StopMove:
                    key = GetKeyByType(AnimationTypes.StartMove);
                    SetBool(key, false);
                    break;




                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;

                case AnimationTypes.TakeDamage:
                    //if (IsPlayingBattleIdle())
                    //    SetTrigger(key);
                    break;

                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Idle:
                    //SetBool(GetAnimationName(AnimationTypes.BattleIdle), false);
                    break;
            
                case AnimationTypes.Victory:
                    SetTrigger(key);
                    break;
            }
        }


        /*bool IsPlayingIdle()
        {
            return Controller.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(m_IdleHash);
        }*/
    }
}

