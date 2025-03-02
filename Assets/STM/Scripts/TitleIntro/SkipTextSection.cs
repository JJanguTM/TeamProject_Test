using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace STM
{
    public class SkipTextSection : MonoBehaviour
    {

        [SerializeField] private PlayableDirector director;
        [SerializeField] private double textEndTime; // PlayableDirector.time의 속성이 double 타입을 사용하여 변수 데이터 타입을 double로 설정했습니다

        private void Awake()
        {
            if (director == null)
            {
                director = GetComponent<PlayableDirector>();
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (director.time < textEndTime)
                {
                    director.time = textEndTime;
                    director.Evaluate();
                }
            }
        }
    }
}