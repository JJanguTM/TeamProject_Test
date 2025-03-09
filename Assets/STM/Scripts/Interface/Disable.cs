using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Disable : MonoBehaviour
    {
        private bool isActivated = true;

        [Header("Camera Shake")]
        [SerializeField] private CameraShakeController cameraShake;
        [SerializeField] private float shakeIntensity = 5;
        [SerializeField] private float shakeTime = 1;

        // ���� �������� ���� ���¿� �°� ����
        private void Start()
        {
            gameObject.SetActive(isActivated);
        }

        // �ܺο��� Ȱ��/��Ȱ�� ��û�� ���� ������ ��� ����
        public void ToggleActiveState(bool state)
        {
            isActivated = state;
            gameObject.SetActive(!isActivated);
            cameraShake.ShakeCamera(shakeIntensity, shakeTime);
        }
    }
}
