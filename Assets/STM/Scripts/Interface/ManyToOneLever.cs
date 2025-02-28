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
                Debug.Log("Lever가 On 상태가 되었습니다.");
            }
            else
            {
                Debug.Log("Lever가 Off 상태가 되었습니다.");
            }
        }

       
        public bool IsOn
        {
            get { return isLeverOn; }
        }
    }
}