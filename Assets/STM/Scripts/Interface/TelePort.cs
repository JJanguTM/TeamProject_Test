using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Teleport : MonoBehaviour, IInteractable
    {
        [Header("Teleport Settings")]
        [SerializeField] private Transform teleportOutput; // 텔레포트 출력 위치
    

        public void OnInteract()
        {
            if (teleportOutput != null)
            {
                // Player의 Transform 위치를 텔레포트 출력 위치로 이동
                Transform playerTransform = FindObjectOfType<PlayerController>().transform;
                playerTransform.position = teleportOutput.position;

            }
            else
            {
                Debug.LogWarning("텔레포트 출력 위치가 설정되지 않았습니다!");
            }
        }
 
    }
}