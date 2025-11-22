using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    public abstract class Skill : ScriptableObject {

        [Header("Skill Settings")]
        public string skillName;
        public float cooldown;

        public abstract void Execute(GameObject caster);

    }

    /*
        How to use:
        1. Create a new script that inherits from Skill.
        2. Implement the Execute method to define the skill's behavior.
        3. Create a ScriptableObject asset of your new skill class via the Unity Editor.
        4. Assign the skill asset to PlayerSkillController.
    */
}