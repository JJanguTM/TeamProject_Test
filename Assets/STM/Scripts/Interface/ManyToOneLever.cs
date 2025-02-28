using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class ManyToOneLever : MonoBehaviour, IInteractable
    {
    
        private bool isLeverOn = false;

        void Start()
        {
         
        }

        void Update()
        {
          
        }

        public void OnInteract()
        {
           
            isLeverOn = !isLeverOn;

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
    }
}