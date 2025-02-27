using System;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class HorizontalMoveObject : MonoBehaviour
    {

        [SerializeField] 
        private float moveSpeed = 2f;    
        [SerializeField] 
        private float leftOffset = 5f;   
        [SerializeField] 
        private float rightOffset = 5f; 

        private Rigidbody2D rb;
        private float leftBound;
        private float rightBound;
        private int direction = -1;        
        private bool isActivated = false;  
        
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

          
            float initX = rb.position.x;
            leftBound = initX - leftOffset;   
            rightBound = initX + rightOffset; 
        }

        private void Update()
        {
            if (isActivated)
            {
             
                float currentX = rb.position.x;

              
                if (currentX <= leftBound && direction < 0)
                {
                    direction = 1;
                }
               
                else if (currentX >= rightBound && direction > 0)
                {
                    direction = -1;
                }

                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
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