using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class ManyToOneObject : MonoBehaviour
    {
     
        [SerializeField]
        private List<STM.ManyToOneLever> leverList;

        private bool isActivated = true;

        void Start()
        {
            gameObject.SetActive(isActivated);
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
                gameObject.SetActive(!isActivated);
                Debug.Log("��� Lever�� On ����! ManyToOneObject ��� ����!");
            }
            else
            {
           
            }
        }
    }
}