using UnityEngine;
using RhytmTD.Animation;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Battle.Entities.Views
{
    public class PlayerEntityAnimationView : AbstractAnimationView
    {
        private int m_MoveHash;

        public override void Initialize()
        {
            base.Initialize();

            m_MoveHash = Animator.StringToHash($"{m_BASE_LAYER}.{GetKeyByType(AnimationTypes.StartMove)}");
        }

        public override void PlayAnimation(AnimationTypes animationType)
        {
            string key = GetKeyByType(animationType);

            switch (animationType)
            {
                case AnimationTypes.Attack:
                    SetTrigger(key);
                    break;

                case AnimationTypes.TakeDamage:
                    if (IsPlaying(m_MoveHash))
                        SetTrigger(key);
                    break;

                case AnimationTypes.Destroy:
                    SetTrigger(key);
                    break;

                case AnimationTypes.StartMove:
                    SetBool(key, true);
                    break;

                case AnimationTypes.StopMove:
                    key = GetKeyByType(AnimationTypes.StartMove);
                    SetBool(key, false);
                    break;

                case AnimationTypes.IncreaseHP:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Show:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Hide:
                    SetTrigger(key);
                    break;

                case AnimationTypes.Victory:
                    SetTrigger(key);
                    break;

                case AnimationTypes.MenuAction:
                    SetTrigger(key);
                    break;
            }
        }

        private bool IsPlaying(int hash)
        {
            return Controller.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(hash);
        }
    }
}

