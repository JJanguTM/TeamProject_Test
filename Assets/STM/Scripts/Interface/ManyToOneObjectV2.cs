using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class ManyToOneObjectV2 : MonoBehaviour
    {
     
        [SerializeField] private List<STM.ManyToOneLever> leverList;

        [SerializeField] private List<STM.ManyToOneLever> forbiddenLeverList;



        private BoxCollider2D col;
        private Renderer rend;

        private bool doorOpened = false;

        void Start()
        {
            col = GetComponent<BoxCollider2D>();
            rend = GetComponent<Renderer>();
        }

        void Update()
        {
            // 체크 결과에 따라 활성/비활성을 갱신
            bool conditionMet = CheckAllConditions();

            if (doorOpened != conditionMet)
            {
                doorOpened = conditionMet;
                col.enabled = !conditionMet;
                rend.enabled = !conditionMet;

                if (conditionMet)
                {
                    Debug.Log("정답 레버들만 On이고, 금지 레버들은 Off! ManyToOneObject 활성화!");
                }
                else
                {
                    Debug.Log("금지 레버 On 발견 혹은 정답 레버 부족! ManyToOneObject 비활성화!");
                }
            }
        }

        private bool CheckAllConditions()
        {
            // 1) requiredLevers 리스트 모두 On 인지
            foreach (var lever in leverList)
            {
                if (!lever.IsOn)
                {
                    return false; 
                }
            }

            // 2) forbiddenLevers 리스트 중 하나라도 On이면 안 됨
            foreach (var lever in forbiddenLeverList)
            {
                if (lever.IsOn)
                {
                    return false; 
                }
            }

            // 여기까지 통과하면 “정답 레버들은 모두 On, 금지 레버는 모두 Off”
            return true;
        }
    }
}