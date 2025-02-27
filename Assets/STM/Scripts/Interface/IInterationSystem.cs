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

        private List<float> distances = new List<float>();
        private Vector2 offset = Vector2.zero;
      
        void Update()
        {
            DetectInteractable();

            if (currentInteractable != null && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F 키 입력 감지됨");
                currentInteractable.OnInteract();
            }

        }

        void DetectInteractable()
        {
            Vector2 centerPosition = new Vector2(transform.position.x, transform.position.y) + offset;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(centerPosition, interactionRange, interactLayer);

            if (hitColliders.Length > 0)
            {
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

         
                if (currentInteractable != nearestInteractable)
                {
                    currentInteractable = nearestInteractable;

                    if (nearestCollider != null) 
                    {
                        Debug.Log($"상호작용 가능한 오브젝트: {nearestCollider.name}");
                    }
                }
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Vector2 centerPosition = new Vector2(transform.position.x, transform.position.y) + offset;
          
            Gizmos.DrawWireSphere(centerPosition, interactionRange);
        }
    }
}
