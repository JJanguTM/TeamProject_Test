using UnityEngine;
using UnityEngine.SceneManagement;

namespace STM
{
    public class DialogueInitiate : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private string dialogID;
        // [SerializeField] private NPCInteract npcReference; // NPCInteract를 사용한다면 주석 해제

        private bool hasInitiated = false;

        private void OnEnable()
        {
            // 스크립트가 활성화될 때 단 한 번만 재생하고 싶다면, if문을 사용.
            // 여러 번 재생이 필요하다면 if문을 제거하거나 다른 타이밍에 reset하는 방식을 적용.
            if (!hasInitiated)
            {
                hasInitiated = true;
                if (dialogueManager != null)
                {
                    // NPC가 필요 없다면 null로 넘기거나, 필요한 경우 npcReference 사용
                    dialogueManager.StartConversation(dialogID);
                }
                else
                {
                    Debug.LogWarning("[DialogueInitiate] DialogueManager가 할당되지 않았습니다.");
                }
            }
        }

        private void Update()
        {
            // DialogueManager가 할당되어 있고,
            // Conversation이 끝났다면(ESC나 finish 등 어떤 이유로든 EndConversation()이 호출됨)
            // 바로 씬 전환
            if (dialogueManager != null && !dialogueManager.isConversationActive)
            {
                // Main 씬으로 넘어가기
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }
}
