using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class NPCInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueManager dialogueManager;

        [Header("대화 시작 LineID")]
        [SerializeField] private string startLineID;

        [Header("플레이어 선택지(대답) LineIDs")]
        [SerializeField] private List<string> choiceLineIDs = new List<string>(); // 유동적인 리스트로 변경

        /// <summary>
        /// 플레이어가 F 키 등을 누르면 실행
        /// </summary>
        public void OnInteract()
        {
            Debug.Log($"[NPCInteract] OnInteract() => StartConversation({startLineID}) on {gameObject.name}");
            if (dialogueManager != null && !string.IsNullOrEmpty(startLineID))
            {
                dialogueManager.StartConversation(startLineID, this);
            }
            else
            {
                Debug.LogWarning($"[NPCInteract] dialogueManager is null or startLineID is empty. startLineID={startLineID}");
            }
        }

        /// <summary>
        /// 유동적으로 LineID를 세팅할 수 있는 메서드
        /// </summary>
        public void SetChoiceLineIDs(List<string> newChoiceLineIDs)
        {
            choiceLineIDs = newChoiceLineIDs;
            Debug.Log($"[NPCInteract] SetChoiceLineIDs() called. New choices: {string.Join(", ", choiceLineIDs)}");
        }

        public Dialogue[] GetChoiceDialogues(DialogueData data)
        {
            Debug.Log($"[NPCInteract] GetChoiceDialogues() called on {gameObject.name}. choiceLineIDs.Count={choiceLineIDs.Count}");

            Dialogue[] arr = new Dialogue[choiceLineIDs.Count];

            for (int i = 0; i < choiceLineIDs.Count; i++)
            {
                arr[i] = data.GetDialogueByID(choiceLineIDs[i]);
                if (arr[i] == null)
                {
                    Debug.LogWarning($"[NPCInteract] choiceLineID={choiceLineIDs[i]} not found in DialogueData!");
                }
                else
                {
                    Debug.Log($"[NPCInteract] Found dialogue for {choiceLineIDs[i]} => nextLine={arr[i].nextLine}");
                }
            }
            return arr;
        }
    }
}
