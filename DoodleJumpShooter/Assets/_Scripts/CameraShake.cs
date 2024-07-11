using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [HideInInspector] public float duration;
    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    public static CameraShake singleton;
    void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        singleton = this;
    } 
    void FixedUpdate()
    {
        if (duration > 0) {
            duration -= Time.fixedDeltaTime;
            if (duration < 0) {
                multiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }
    public void Shake(float duration, float intensity) {
        this.duration = duration;
        multiChannelPerlin.m_AmplitudeGain = intensity;
    }
}
