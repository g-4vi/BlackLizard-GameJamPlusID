using System.Collections.Generic;
using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    /// <summary>
    /// Database of all skills in the project.
    /// Used to manage and reference all available skills. Like for example in skill selection UI.
    /// </summary>
    [CreateAssetMenu(fileName = "Skill Database", menuName = "GameJamPlus/Skill Database")]
    public class SkillDatabase : ScriptableObject {
        public List<BaseSkill> allSkills;

#if UNITY_EDITOR
        // Helper method to find all Skill assets in the project
        [ContextMenu("Find All Skills in Project")]
        void FindAllSkills() {
            allSkills = new List<BaseSkill>();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Skill");

            if (guids.Length == 0) {
                Debug.LogWarning("No 'Skill' assets found in project.");
                return;
            }

            foreach (string guid in guids) {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                BaseSkill skill = UnityEditor.AssetDatabase.LoadAssetAtPath<BaseSkill>(path);
                if (skill != null) {
                    allSkills.Add(skill);
                }
            }

            Debug.Log($"[SkillDatabase] Found and registered {allSkills.Count} skills.");

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}