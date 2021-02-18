using UnityEngine;
using UnityEngine.UI;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Animation.Tools
{
    public class AnimationSceneUIManager : MonoBehaviour
    {
        public RectTransform Parent;
        public Button AnimationButtonPrefab;

        private AbstractAnimationView m_AnimationController;

        public void AddButton(AnimationTypes type, string key)
        {
            Button btn = Instantiate(AnimationButtonPrefab);
            btn.GetComponent<RectTransform>().SetParent(Parent);
            btn.transform.localScale = Vector3.one;
            btn.onClick.AddListener(() => { m_AnimationController.PlayAnimation(type); });

            Text text = btn.GetComponentInChildren<Text>();
            text.text = $"<b>{type}</b>\n{key}"; 
        }

        public void SetController(AbstractAnimationView animationController)
        {
            m_AnimationController = animationController;
        }
    }
}
