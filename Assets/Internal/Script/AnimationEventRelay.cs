using GameJamPlus;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    PlayerSkillController skillController;
    SpiderBehaviour spiderBehaviour;
    
    void Start()
    {
        skillController = GetComponentInParent<PlayerSkillController>();

        spiderBehaviour = GetComponentInParent<SpiderBehaviour>();
    }

    public void SpawnFireEvent()
    {
        if(skillController != null)
            skillController.ExecuteSkill();
    }

    public void ShootWeb()
    {
        if(spiderBehaviour != null)
            spiderBehaviour.ActivateShooting();
    }
}
