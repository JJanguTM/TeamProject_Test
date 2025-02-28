using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class IInterationSystem : MonoBehaviour
    {
        private IInteractable currentInteractable;

        [SerializeField]
        private float interactionRange = 3f;
        [SerializeField]
        private LayerMask interactLayer;

        private Vector2 offset = Vector2.zero;

        void Update()
        {
            DetectInteractable();

            // currentInteractable이 존재할 때만 F키 상호작용 가능
            if (currentInteractable != null && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F 키 입력 감지됨");
                currentInteractable.OnInteract();
            }
        }

        void DetectInteractable()
        {
            Vector2 centerPosition = (Vector2)transform.position + offset;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(centerPosition, interactionRange, interactLayer);

            // 현재 프레임에서 감지된 상호작용 가능 오브젝트가 없을 경우, null로 초기화
            if (hitColliders.Length == 0)
            {
                currentInteractable = null;
                return;
            }

            float minDistance = float.MaxValue;
            IInteractable nearestInteractable = null;
            Collider2D nearestCollider = null;

            foreach (Collider2D hitCollider in hitColliders)
            {
                Debug.Log($"감지된 오브젝트: {hitCollider.name}");

                float distance = Vector2.Distance(centerPosition, hitCollider.transform.position);
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();

                if (interactable != null && distance < minDistance)
                {
                    minDistance = distance;
                    nearestInteractable = interactable;
                    nearestCollider = hitCollider;
                }
            }

            // 현재 주시 중인(currentInteractable) 오브젝트가 바뀌었는지 확인
            if (currentInteractable != nearestInteractable)
            {
                currentInteractable = nearestInteractable;

                if (nearestCollider != null)
                {
                    Debug.Log($"상호작용 가능한 오브젝트: {nearestCollider.name}");
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector2 centerPosition = (Vector2)transform.position + offset;
            Gizmos.DrawWireSphere(centerPosition, interactionRange);
        }
    }
}
