using RhytmTD.Assets.Abstract;
using RhytmTD.UI.Components;
using RhytmTD.UI.Widget;
using UnityEngine;

namespace RhytmTD.Assets.Battle
{
    [CreateAssetMenu(fileName = "New UI Assets", menuName = "Assets/UI Assets", order = 101)]
    public class UIAssets : PrefabAssets
    {
        public UIWidget_Spell UIWidgetSpellPrefab;
        public UIComponent_SpellSequenceItem UIComponentSpellSequenceItemPrefab;

        public override void Initialize()
        {
        }
    }
}
