using System.Collections.Generic;
using UnityEngine;
using static CoreFramework.EnumsCollection;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New UISprite Assets", menuName = "Assets/UISprite Assets", order = 101)]
    public class UISpriteAssets : ScriptableObject
    {
        public List<SkillIconSpriteData> SkillIconSprites;

        private Dictionary<SkillTypeID, (Sprite sprite, Color color)> m_SkillIconSprites;

        public void Initialize()
        {
            if (m_SkillIconSprites == null)
            {
                m_SkillIconSprites = new Dictionary<SkillTypeID, (Sprite sprite, Color color)>();
                for (int i = 0; i < SkillIconSprites.Count; i++)
                {
                    m_SkillIconSprites[SkillIconSprites[i].ID] = (SkillIconSprites[i].Sprite, SkillIconSprites[i].Color);
                }
            }
        }

        public (Sprite sprite, Color color) GetSkillIconSprite(int skillTypeID)
        {
            SkillTypeID id = (SkillTypeID)skillTypeID;
            if (m_SkillIconSprites.ContainsKey(id))
            {
                return m_SkillIconSprites[id];
            }

            return (null, Color.white);
        }

        [System.Serializable]
        public class SkillIconSpriteData
        {
            public SkillTypeID ID;
            public Sprite Sprite;
            public Color Color;
        }
    }
}
