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
            // 대화 중이면 이동과 스프라이트 갱신 중단
            if (dialogueManager.IsConversationActive)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                return;
            }

            HandleJump();    
            UpdateSprite();  
        }

        private void FixedUpdate()
        {
            // 대화 중이면 이동 중단
            if (dialogueManager.IsConversationActive)
                return;

            CheckGrounded();  // 땅에 닿았는지 확인

            // 땅에 있을 때만 이동 처리
            if (isGrounded)
            {
                MoveCharacter(InputSystem.Singleton.MoveInput);
            }
            
            if (rb.velocity.y > 0 || rb.velocity.y < 0)
            {
                if(!isGrounded)
                    ani.enabled = false;
            }
        }

        /// <summary>
        /// 좌우 이동 처리
        /// </summary>
        private void MoveCharacter(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                // 만약 점프 중이 아니면 달리기 애니메이션 재생
                if (!isJumping)
                {
                    ani.SetBool("IsRun", true);
                }
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

                // 캐릭터 좌우 반전 : rotation.y 를 180 <-> 0 을 왔다갔다 하게 함 by 휘익 0310
                // 이렇게 하면 단순 스프라이트만 뒤집는게 아니라 캐릭터의 실제 transform.right를 뒤집게 됨
                // Cinemachine의 X Offset은 transform.right을 추적 

                
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
                // 이동 입력이 없으면 달리기 애니메이션 끔
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

            // 겹치는 Collider2D가 있으면 땅으로 판정
            Collider2D groundCollider = Physics2D.OverlapBox(groundCheckPosition, boxSize, 0f, groundLayer);
            isGrounded = groundCollider != null;

            // 땅에 착지하면 점프 상태 해제 + 기본 스프라이트 복귀
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
            // 점프 키를 눌렀고, 땅에 닿아 있다면
            if (InputSystem.Singleton.Jump && isGrounded)
            {
                // 달리기 애니메이션 끄기
                ani.SetBool("IsRun", false);
                
                // 점프 상태로 전환
                isJumping = true;

                // 점프 스프라이트 적용
                sp.sprite = jumpSprite;

                // 점프 위력 부여
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

       
        private void UpdateSprite()
        {
         
            //Debug.Log($"isGrounded: {isGrounded}, velocityY: {rb.velocity.y}, isJumping: {isJumping}");

            // 만약 땅에서 떨어져있다면(점프 상태) => 상승/최고점/낙하 스프라이트
            if (!isGrounded)
            {
                float yVelocity = rb.velocity.y;

                // 상승 중
                if (yVelocity > 0.1f)
                {
                    sp.sprite = jumpSprite;
                }
                // 최고점
                else if (yVelocity <= 0.1f && yVelocity >= -0.1f && !reachedApex)
                {
                    reachedApex = true;
                    sp.sprite = MaxhighSprite;
                }
                // 낙하 중
                else if (yVelocity < -0.1f)
                {
                    float t = Mathf.Repeat(Time.time, fallSpriteChangeInterval * 2f);
                    sp.sprite = (t < fallSpriteChangeInterval) ? fallSprite1 : fallSprite2;
                }
            }
            else
            {
                // 땅에 닿아있다면 기본 스프라이트
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

            // 씬 뷰에서 지면 체크 범위 확인용
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPosition, boxSize);
        }
    }
}
