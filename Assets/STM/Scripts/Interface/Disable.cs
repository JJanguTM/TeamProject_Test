using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Disable : MonoBehaviour
    {
        private bool isActivated = true;

        // ���� �������� ���� ���¿� �°� ����
        private void Start()
        {
            gameObject.SetActive(isActivated);
        }

        // �ܺο��� Ȱ��/��Ȱ�� ��û�� ���� ������ ��� ����
        public void ToggleActiveState(bool state)
        {
            isActivated = state;
            gameObject.SetActive(!isActivated);
        }
    }
}
