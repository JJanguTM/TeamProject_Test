using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Teleport : MonoBehaviour, IInteractable
    {
        [Header("Teleport Settings")]
        [SerializeField] private Transform teleportOutput;

        [SerializeField] private int delay = 1;// 텔레포트 출력 위치

        private Camera mainCamera;
        private PlayerController playerController;
        private bool isTeleporting = false;
        private float teleportStartTime = 0f;
        private void Start()
        {
            mainCamera = Camera.main;
            playerController = FindObjectOfType<PlayerController>();

            if (mainCamera == null)
                Debug.LogError("메인 카메라가 없습니다!");

            if (playerController == null)
                Debug.LogError("PlayerController가 없습니다!");
        }

        private void Update()
        {
            if (isTeleporting)
            {
                float elapsedTime = Time.time - teleportStartTime;

                if (elapsedTime >= delay)
                {
                   
                    playerController.transform.position = teleportOutput.position;

                   
                    mainCamera.cullingMask = -1; 

                 
                    playerController.enabled = true;

           
                    isTeleporting = false;
                }
            }
        }
        public void OnInteract()
        {
            if (teleportOutput != null && mainCamera != null && playerController != null && !isTeleporting)
            {
               
                isTeleporting = true;
                teleportStartTime = Time.time;
                playerController.enabled = false;
 
                mainCamera.cullingMask = 0; // Nothing
            }
            else if (isTeleporting)
            {
                Debug.LogWarning("이미 텔레포트 중입니다!");
            }
            else
            {
                Debug.LogWarning("텔레포트 출력 위치 또는 카메라가 설정되지 않았습니다!");
            }
        }

    }
}