using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float groundCheckWidthMultiplier = 0.9f; // 지면 체크 너비 (캐릭터 크기 대비)
        [SerializeField] private float groundCheckHeight = 0.1f;         // 지면 체크 높이
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundadjusted = 0.1f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float speed = 3f;

        // 대화 시스템
        private DialogueManager dialogueManager;

        // 컴포넌트
        private Rigidbody2D rb;
        private Animator ani;
        private SpriteRenderer sp;

        // 상태값
        private bool isGrounded;
        private bool isJumping;
        public bool isFacingRight { get; private set; } // 외부에서 접근만 가능, 수정은 불가
        private bool reachedApex;

        [Header("Sprites")]
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite jumpSprite;
        [SerializeField] private Sprite MaxhighSprite;
        [SerializeField] private Sprite fallSprite1;
        [SerializeField] private Sprite fallSprite2;

        [Header("Fall Sprite Settings")]
        [SerializeField] private float fallSpriteChangeInterval = 0.1f;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sp = GetComponent<SpriteRenderer>();
            ani = GetComponent<Animator>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        private void Update()
        {
            // 대화 중이면 입력을 무시하고, 이동 벡터를 0으로 설정
            if (dialogueManager != null && dialogueManager.isConversationActive)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                return;
            }

            HandleJump();    
            UpdateSprite();  
        }

        private void FixedUpdate()
        {
            // 대화 중이면 물리적 이동 처리도 하지 않음
            if (dialogueManager != null && dialogueManager.isConversationActive)
                return;

            CheckGrounded();  // 땅에 닿았는지 확인

            // 땅에 있을 때만 이동 처리
            if (isGrounded)
            {
                MoveCharacter(InputSystem.Singleton.MoveInput);
            }
            
            if (rb.velocity.y > 0 || rb.velocity.y < 0)
            {
                if (!isGrounded)
                    ani.enabled = false;
            }
        }

        /// <summary>
        /// 좌우 이동 처리
        /// </summary>
        private void MoveCharacter(Vector2 direction)
        {
            // 대화 중이 아니고 입력이 있을 때만 실행
            if (direction != Vector2.zero)
            {
                if (!isJumping)
                {
                    ani.SetBool("IsRun", true);
                }
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

                isFacingRight = (direction.x < 0) ? true : false;
                
                if (isFacingRight)
                {
                    Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                }
                else
                {
                    Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                }
            }
            else
            {
                ani.SetBool("IsRun", false);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        /// <summary>
        /// 땅에 닿았는지 확인
        /// </summary>
        private void CheckGrounded()
        {
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

            Collider2D groundCollider = Physics2D.OverlapBox(groundCheckPosition, boxSize, 0f, groundLayer);
            isGrounded = groundCollider != null;

            if (isGrounded)
            {
                isJumping = false;
                reachedApex = false; 
                ani.enabled = true;
            }
        }

        /// <summary>
        /// 점프 처리
        /// </summary>
        private void HandleJump()
        {
            // 대화 중이 아니라면 점프 입력 처리
            if (InputSystem.Singleton.Jump && isGrounded)
            {
                ani.SetBool("IsRun", false);
                isJumping = true;
                sp.sprite = jumpSprite;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        private void UpdateSprite()
        {
            if (!isGrounded)
            {
                float yVelocity = rb.velocity.y;

                if (yVelocity > 0.1f)
                {
                    sp.sprite = jumpSprite;
                }
                else if (yVelocity <= 0.1f && yVelocity >= -0.1f && !reachedApex)
                {
                    reachedApex = true;
                    sp.sprite = MaxhighSprite;
                }
                else if (yVelocity < -0.1f)
                {
                    float t = Mathf.Repeat(Time.time, fallSpriteChangeInterval * 2f);
                    sp.sprite = (t < fallSpriteChangeInterval) ? fallSprite1 : fallSprite2;
                }
            }
            else
            {
                sp.sprite = defaultSprite;
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