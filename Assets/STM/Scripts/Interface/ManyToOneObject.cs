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
            // �ʿ信 ���� �ʱ�ȭ ����
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
              
                Debug.Log("��� Lever�� On ����! ManyToOneObject ��� ����!");
            }
            else
            {
           
            }
        }
    }
}