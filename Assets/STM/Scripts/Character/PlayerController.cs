using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace STM
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundadjusted = 0.1f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float Speed = 3f;

        private Rigidbody2D rb;
        private bool Jump;
        private Vector2 MoveInput;
        private bool isGrounded;
        private Animator ani;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
        }

        private void Update()
        {

            HandleJump();
        }

        private void FixedUpdate()
        {

            Vector2 input = InputSystem.Singleton.MoveInput;
            CheckGrounded();

            if (isGrounded)
            {
                MoveCharacter(input);
            }
        }

        private void MoveCharacter(Vector2 direction)
        {
            ani.SetBool("IsRun", direction != Vector2.zero);

            if (direction != Vector2.zero)
            {
                Vector2 velocity = rb.velocity;
                velocity.x = direction.x * Speed ;
                rb.velocity = velocity;

            if (direction.x < 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            else if (direction.x > 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
            else
            {
                
                Vector2 velocity = rb.velocity;
                 velocity.x = 0;
                rb.velocity = velocity;
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

            Collider2D groundCollider = Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayer);
            isGrounded = groundCollider != null;
        }

        private void HandleJump()
        {
            if (InputSystem.Singleton.Jump && isGrounded)
            {
                Vector2 jumpVelocity = new Vector2(rb.velocity.x, jumpForce);
                rb.velocity = jumpVelocity;
                CheckLeverActivation();
            }
        }

        private void CheckLeverActivation()
        {
          
            Lever[] levers = FindObjectsOfType<Lever>();

            foreach (Lever lever in levers)
            {
                if (lever.CompareTag("BlackLever")) 
                {
                    lever.ToggleLever(); 
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            if (rb != null)
            {
                Vector2 groundCheckPosition = new Vector2(
                    transform.position.x,
                    transform.position.y - rb.GetComponent<Collider2D>().bounds.extents.y - groundadjusted
                );

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);
            }
        }
    }
}
