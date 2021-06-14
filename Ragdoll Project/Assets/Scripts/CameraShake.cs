using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    public static CameraShake Instance {get; private set;}

    CinemachineVirtualCamera virtualCam;
    float shakeTimer;

    private void Awake()
    {
        Instance = this;
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }


    public void ShakeCamera(float intensity,float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    private void Update()
    {
        if(shakeTimer >0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer<=0)
            {
                CinemachineBasicMultiChannelPerlin perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0;

            }
        }
    }
}
