using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class Disable : MonoBehaviour
    {
        private bool isActivated = false;

        // 시작 시점에서 현재 상태에 맞게 설정
        private void Start()
        {
            gameObject.SetActive(isActivated);
        }

        // 외부에서 활성/비활성 요청을 받을 때마다 즉시 적용
        public void ToggleActiveState(bool state)
        {
            isActivated = state;
            gameObject.SetActive(isActivated);
        }
    }
}
