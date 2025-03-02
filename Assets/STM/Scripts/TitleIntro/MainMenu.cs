using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace STM
{

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject panel;

        private void Start()
        {
            panel.SetActive(false);
        }

        private void Update()
        {
            if (panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                panel.SetActive(false);
            }
        }

        public void  PlayGame()
        {
            SceneManager.LoadScene("Intro"); // SceneManager가 큰 따옴표 안에 있는 이름과 일치하는 Scene을 불러옵니다.
        }

        public void QuitGame()
        {
            Application.Quit(); // 게임을 종료합니다.
        }

        public void InputInfo()
        {
            panel.SetActive(true);
        }
    }
}
