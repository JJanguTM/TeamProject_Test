using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float groundCheckWidthMultiplier = 0.9f; // 지면 체크 너비 (캐릭터 크기 대비)
        [SerializeField] private float groundCheckHeight = 0.1f; // 지면 체크 높이
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundadjusted = 0.1f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float speed = 3f;
        private DialogueManager dialogueManager;

        private Rigidbody2D rb;
        private bool isGrounded;
        private Animator ani;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        private void Update()
        {
            if (dialogueManager.IsConversationActive)
            {
                ani.SetBool("IsRun", false);
                rb.velocity = new Vector2(0, rb.velocity.y);  // 이동 정지
                return;
            }
            HandleJump();
        }

        private void FixedUpdate()
        {
            if (dialogueManager.IsConversationActive)
                return; 

            CheckGrounded();
            if (isGrounded)  
            {
                MoveCharacter(InputSystem.Singleton.MoveInput);
            }
            
        }

        private void MoveCharacter(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                ani.SetBool("IsRun", true);
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

                // 캐릭터 방향 전환
                Vector3 scale = transform.localScale;
                scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            else
            {
                ani.SetBool("IsRun", false);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        private void CheckGrounded()
        {
            if (rb == null) return;

            Collider2D collider = GetComponent<Collider2D>();

            Vector2 groundCheckPosition = new Vector2(
                transform.position.x,
                transform.position.y - collider.bounds.extents.y - groundadjusted
            );

            Vector2 boxSize = new Vector2(
                collider.bounds.size.x * groundCheckWidthMultiplier, // 가로 너비는 캐릭터 크기의 90%
                groundCheckHeight // 감지 높이
            );

            Collider2D groundCollider = Physics2D.OverlapBox(groundCheckPosition, boxSize, 0f, groundLayer);
            isGrounded = groundCollider != null;
        }

        private void HandleJump()
        {
            if (InputSystem.Singleton.Jump && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                CheckLeverActivation();
            }
        }

        private void CheckLeverActivation()
        {
            foreach (Lever lever in FindObjectsOfType<Lever>())
            {
                if (lever.CompareTag("BlackLever")) 
                {
                    lever.ToggleLever(); 
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (rb == null) return;

            Collider2D collider = GetComponent<Collider2D>();

            Vector2 groundCheckPosition = new Vector2(
                transform.position.x,
                transform.position.y - collider.bounds.extents.y - groundadjusted
            );

            Vector2 boxSize = new Vector2(
                collider.bounds.size.x * groundCheckWidthMultiplier,
                groundCheckHeight
            );

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPosition, boxSize);
        }
    }
}
