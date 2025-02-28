using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Teleport : MonoBehaviour, IInteractable
    {
        [Header("Teleport Settings")]
        [SerializeField] private Transform teleportOutput; // �ڷ���Ʈ ��� ��ġ
    

        public void OnInteract()
        {
            if (teleportOutput != null)
            {
                // Player�� Transform ��ġ�� �ڷ���Ʈ ��� ��ġ�� �̵�
                Transform playerTransform = FindObjectOfType<PlayerController>().transform;
                playerTransform.position = teleportOutput.position;

            }
            else
            {
                Debug.LogWarning("�ڷ���Ʈ ��� ��ġ�� �������� �ʾҽ��ϴ�!");
            }
        }
 
    }
}