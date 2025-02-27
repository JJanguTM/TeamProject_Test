using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class VerticalMoveObject : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 2f;
        [SerializeField]
        private float downOffset = 5f;
        [SerializeField]
        private float upOffset = 5f;

        private Rigidbody2D rb;
        private float downBound;
        private float upBound;
        private int direction = -1;        // 현재 이동 방향 (-1: 왼쪽, +1: 오른쪽)
        private bool isActivated = false;  // Lever ON/OFF 상태

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();


            float initY = rb.position.y;
            downBound = initY - downOffset;
            upBound = initY + upOffset;
        }

        private void Update()
        {
            if (isActivated)
            {

                float currentY = rb.position.y;


                if (currentY <= downBound && direction < 0)
                {
                    direction = 1;
                }

                else if (currentY >= upBound && direction > 0)
                {
                    direction = -1;
                }

                rb.velocity = new Vector2(rb.velocity.x, direction * moveSpeed);
            }
            else
            {

                rb.velocity = Vector2.zero;
            }
        }


        public void ToggleMovement(bool state)
        {
            isActivated = state;
        }
    }
}