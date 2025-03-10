using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class NPCInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueManager dialogueManager;

        [Header("대화 시작 DialogID")]
        [SerializeField] private string startDialogID;

        [Header("플레이어 선택지(대답) DialogIDs")]
        [SerializeField] private List<string> choiceDialogIDs = new List<string>();

        /// <summary>
        /// 플레이어가 F 키 등을 누르면 실행됩니다.
        /// 지정한 startDialogID를 기반으로 대화를 시작합니다.
        /// </summary>
        public void OnInteract()
        {
            Debug.Log($"[NPCInteract] OnInteract() => StartConversation({startDialogID}) on {gameObject.name}");
            if (dialogueManager != null && !string.IsNullOrEmpty(startDialogID))
            {
                dialogueManager.StartConversation(startDialogID);
            }
            else
            {
                Debug.LogWarning($"[NPCInteract] dialogueManager가 null이거나 startDialogID가 비어있습니다. startDialogID={startDialogID}");
            }
        }

        /// <summary>
        /// 선택지 DialogID들을 동적으로 설정합니다.
        /// </summary>
        public void SetChoiceDialogIDs(List<string> newChoiceDialogIDs)
        {
            choiceDialogIDs = newChoiceDialogIDs;
            Debug.Log($"[NPCInteract] SetChoiceDialogIDs() 호출됨. 새 선택지: {string.Join(", ", choiceDialogIDs)}");
        }

        /// <summary>
        /// DialogueManager를 통해 선택지 대화 데이터를 반환합니다.
        /// </summary>
        public Dialogue[] GetChoiceDialogues(DialogueManager manager)
        {
            Debug.Log($"[NPCInteract] GetChoiceDialogues() 호출됨 on {gameObject.name}. 선택지 개수: {choiceDialogIDs.Count}");

            Dialogue[] arr = new Dialogue[choiceDialogIDs.Count];

            for (int i = 0; i < choiceDialogIDs.Count; i++)
            {
                arr[i] = manager.GetDialogueByID(choiceDialogIDs[i]);
                if (arr[i] == null)
                {
                    Debug.LogWarning($"[NPCInteract] choiceDialogID={choiceDialogIDs[i]}에 해당하는 대화를 DialogueManager에서 찾을 수 없습니다!");
                }
                else
                {
                    Debug.Log($"[NPCInteract] 선택지 대화 발견: {choiceDialogIDs[i]} => nextDialogID={arr[i].nextDialogID}");
                }
            }
            return arr;
        }
    }
}