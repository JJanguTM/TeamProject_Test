using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class FinalDoor : MonoBehaviour, IInteractable
    {
        [Header("🎯 이동시킬 오브젝트")]
        [SerializeField] private GameObject objectToMove; // 이동시킬 오브젝트 (스프라이트가 비활성화된 상태)

        [Header("🎯 애니메이터")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject ThankyouScreen;

        [Header("🎯 실행할 애니메이션 이름")]
        [SerializeField] private string animationTriggerName = "Open";

        private bool isActivated = false;

        void Start()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        public void OnInteract()
        {
            if (isActivated) return;  // 중복 실행 방지

            isActivated = true;  // 중복 실행 방지

            // 🛑 스프라이트 비활성화
            if (objectToMove != null)
            {
                SpriteRenderer spriteRenderer = objectToMove.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;

                // 🛑 즉시 위치 이동 (선형 이동 X)
                objectToMove.transform.position = transform.position;

                // 🛑 위치 고정 (Rigidbody가 있을 경우 이동 방지)
                Rigidbody2D rb = objectToMove.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;  // 속도 초기화
                    rb.isKinematic = true;       // 물리적 영향 차단
                }

                // 🛑 애니메이션 실행
                ExecuteInteraction();
            }
            else
            {
                Debug.LogWarning("[FinalDoor] 이동시킬 오브젝트가 설정되지 않았습니다!");
            }
        }

        private void ExecuteInteraction()
        {
            // 🛑 애니메이션 실행
            if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
            {
                animator.SetTrigger(animationTriggerName);
                Debug.Log($"[FinalDoor] {animationTriggerName} 애니메이션 실행!");
                StartCoroutine(ShowThankyouScreen());
            }
            else
            {
                Debug.LogWarning("[FinalDoor] 애니메이터 또는 애니메이션 트리거가 설정되지 않았습니다!");
            }
        }

        private IEnumerator ShowThankyouScreen()
        {
            yield return new WaitForSeconds(5.5f);
            ThankyouScreen.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
