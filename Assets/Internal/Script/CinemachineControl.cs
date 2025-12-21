using Unity.Cinemachine;
using UnityEngine;

public class CinemachineControl : Singleton<CinemachineControl>
{
    CinemachineCamera vmCam;
    protected override void Awake()
    {
        base.Awake();
        vmCam = Instance.GetComponent<CinemachineCamera>();
    }
    public void SetTarget(Transform target)
    {
        if (vmCam == null) return;

        vmCam.Follow = target.transform;
        vmCam.LookAt = target.transform;

        var confiner = vmCam.GetComponent<CinemachineConfiner2D>();
        if (confiner != null)
            confiner.InvalidateBoundingShapeCache();
    }
        
}
