using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace STM
{
    public class NextSceneLoader : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}

