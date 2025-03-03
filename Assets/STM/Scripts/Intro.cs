using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Text textUI;
    [SerializeField, TextArea] private string[] texts;

    private int index;

    private void Start()
    {
        Show();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Show();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }

    private void Show()
    {
        if (index < texts.Length)
        {
            textUI.text = texts[index];
            animator.Play("Show", -1, 0f);
            index++;
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
