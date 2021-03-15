using UnityEditor;

namespace RhytmTD.Editor
{
    public static class ScriptDefinceSymbolsSelector
    {
        private const string m_TARGET_BASED_ATTACK_DEFINE = "TARGET_BASED_ATTACK";
        private const string m_DIRECTION_BASED_ATTACK_DEFINE = "DIRECTION_BASED_ATTACK";

        private static BuildTargetGroup m_TargetGroup = BuildTargetGroup.Standalone | BuildTargetGroup.Android;

        [MenuItem("Tools/Define Editor/Set Target Based Attack")]
        public static void SetTargetBasedAttack()
        {
            if (TryReplaceOrAppendDefine(m_DIRECTION_BASED_ATTACK_DEFINE, m_TARGET_BASED_ATTACK_DEFINE, out string defineSymbols))
                PlayerSettings.SetScriptingDefineSymbolsForGroup(m_TargetGroup, defineSymbols);
        }


        [MenuItem("Tools/Define Editor/Set Direction Based Attack")]
        public static void SetDirectionDiretionBasedAttack()
        {
            if (TryReplaceOrAppendDefine(m_TARGET_BASED_ATTACK_DEFINE, m_DIRECTION_BASED_ATTACK_DEFINE, out string defineSymbols))
                PlayerSettings.SetScriptingDefineSymbolsForGroup(m_TargetGroup, defineSymbols);
        }

        private static bool TryReplaceOrAppendDefine(string oldDefine, string newDefine, out string defineSymbols)
        {
            defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(m_TargetGroup);

            if (oldDefine == newDefine || defineSymbols.Contains(newDefine))
                return false;

            if (defineSymbols.Contains(oldDefine))
                defineSymbols = defineSymbols.Replace(oldDefine, newDefine);
            else
                defineSymbols += $";{newDefine}";

            return true;
        }
    }
}
