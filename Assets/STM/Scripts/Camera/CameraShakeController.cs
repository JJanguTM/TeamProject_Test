using UnityEngine;
using Cinemachine;


//카메라 쉐이크 컨트롤러 스크립트입니다 by 휘익, 0310

public class CameraShakeController : MonoBehaviour
{
    private CinemachineVirtualCamera vcamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private float shakeTimer = 0f;

    private void Awake()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = vcamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ResetIntensity();
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime; 
            if (shakeTimer <= 0f)
            {
                ResetIntensity();
            }
        }
    }
    // Coroutine 대신 Time.deltaTime을 이용했습니다.
    //Time.deltaTime의 누적 시간을 추적하여 해당 시간이 shakeTimer보다 길어지면 ResetIntensity 매서드를 실행합니다

    public void ShakeCamera(float intensity, float shakeTime)
    {
        perlinNoise.m_AmplitudeGain = intensity;
        shakeTimer = shakeTime;
    }

    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }
}