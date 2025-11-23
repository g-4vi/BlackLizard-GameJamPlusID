using GameJamPlus;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    PlayerSkillController skillController;
    
    void Start()
    {
        skillController = GetComponentInParent<PlayerSkillController>();
    }

    public void SpawnFireEvent()
    {
        skillController.ExecuteSkill();
    }
}
