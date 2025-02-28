using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class ManyToOneLever : MonoBehaviour, IInteractable
    {
    
        private bool isLeverOn = false;

        private SpriteRenderer spriteRenderer;
        private Color redColor = Color.red;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = redColor;
        }

        void Update()
        {
          
        }

        public void OnInteract()
        {
           
            isLeverOn = !isLeverOn;
            FlipSprite();

            if (isLeverOn)
            {
                Debug.Log("Lever�� On ���°� �Ǿ����ϴ�.");
            }
            else
            {
                Debug.Log("Lever�� Off ���°� �Ǿ����ϴ�.");
            }
        }

       
        public bool IsOn
        {
            get { return isLeverOn; }
        }

        private void FlipSprite() // 레버와 상호작용 시 레버 스프라이트의 방향을 바꾸는 메서드입니다 by 휘익
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}