using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class ManyToOneObject : MonoBehaviour
    {
     
        [SerializeField]
        private List<STM.ManyToOneLever> leverList;

        void Start()
        {
            // 필요에 따라 초기화 로직
        }

        void Update()
        {
           
            CheckAllLeverOn();
        }

        private void CheckAllLeverOn()
        {
          
            bool allOn = true;

            foreach (var lever in leverList)
            {
                if (!lever.IsOn)
                {
                    allOn = false;
                    break;
                }
            }

            if (allOn)
            {
              
                Debug.Log("모든 Lever가 On 상태! ManyToOneObject 기능 실행!");
            }
            else
            {
           
            }
        }
    }
}