using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace STM
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueData dialogueData;

        [Header("NPC Dialogue UI")]
        [SerializeField] private GameObject npcPanel;
        [SerializeField] private Text npcNameText;
        [SerializeField] private Text npcDialogueText;
        [SerializeField] private Image npcPortraitImage;

        [Header("Typing Settings")]
        [SerializeField] private float typingSpeed = 0.05f;

        [Header("Player Choice UI")]
        [SerializeField] private GameObject playerChoicePanel;
        [SerializeField] private Text playerNameText;           
        [SerializeField] private Image playerPortraitImage;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private Text[] choiceButtonTexts;
        public bool IsConversationActive { get; private set; } = false;
        private NPCInteract currentNPC;
        private Dialogue currentDialogue;
        private int currentLineIndex = 0;
        private bool isConversationActive = false;

        private bool isTyping = false;
        private float typingTimer = 0f;
        private int charIndex = 0;
        private string fullText = "";

        private bool isInChoiceState = false;

        void Start()
        {
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(false);
         
        }

        void Update()
        {
            if (!isConversationActive) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndConversation();
                return;
            }

            if (isTyping)
            {
                typingTimer += Time.deltaTime;
                if (typingTimer >= typingSpeed)
                {
                    typingTimer = 0f;
                    if (charIndex < fullText.Length)
                    {
                        npcDialogueText.text += fullText[charIndex];
                        charIndex++;
                    }
                    else
                    {
                        isTyping = false;
                    }
                }
            }

            if (!isInChoiceState && Input.GetKeyDown(KeyCode.Return))
            {
                OnClickNextLine();
            }
        }

        public void StartConversation(string lineID, NPCInteract npc)
        {
            currentNPC = npc;
            isConversationActive = true;
            isInChoiceState = false;
            IsConversationActive = true;

            Dialogue d = dialogueData.GetDialogueByID(lineID);
            if (d == null)
            {
                EndConversation();
                return;
            }

            currentDialogue = d;
            currentLineIndex = 0;
            ShowCurrentLine();
        }

        private void ShowCurrentLine()
        {
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(false);
            isInChoiceState = false;

            if (currentDialogue == null)
            {
                EndConversation();
                return;
            }

            if (currentLineIndex >= currentDialogue.lines.Length)
            {
                string nextLineID = currentDialogue.nextLine;

                if (string.IsNullOrEmpty(nextLineID))
                {
                    EndConversation();
                    return;
                }

                if (nextLineID == "finish")
                {
                    EndConversation();
                    return;
                }

                if (nextLineID.StartsWith("answer"))
                {
                    ShowPlayerChoices(nextLineID);
                    return;
                }

                Dialogue nextDialogue = dialogueData.GetDialogueByID(nextLineID);
                if (nextDialogue != null && nextDialogue.dialogueType == "1")
                {
                    StartConversation(nextLineID, currentNPC);
                    return;
                }

                StartConversation(nextLineID, currentNPC);
                return;
            }

            if (currentDialogue.dialogueType == "0")
            {
                npcPanel.SetActive(true);
                npcNameText.text = currentDialogue.charName;

                if (!string.IsNullOrEmpty(currentDialogue.charPortrait))
                {
                    Sprite sp = Resources.Load<Sprite>(currentDialogue.charPortrait);
                    if (sp) npcPortraitImage.sprite = sp;
                }

                fullText = currentDialogue.lines[currentLineIndex];
                npcDialogueText.text = "";
                charIndex = 0;
                typingTimer = 0f;
                isTyping = true;
            }
        }

        public void OnClickNextLine()
        {
            if (!isConversationActive || currentDialogue == null) return;

            if (isTyping)
            {
                isTyping = false;
                npcDialogueText.text = fullText;
                return;
            }

            currentLineIndex++;

            if (currentLineIndex >= currentDialogue.lines.Length)
            {
                string nextLineID = currentDialogue.nextLine;

                if (string.IsNullOrEmpty(nextLineID))
                {
                    EndConversation();
                    return;
                }

                if (nextLineID == "finish")
                {
                    EndConversation();
                    return;
                }

                if (nextLineID.StartsWith("answer"))
                {
                    ShowPlayerChoices(nextLineID);
                    return;
                }

                StartConversation(nextLineID, currentNPC);
                return;
            }

            ShowCurrentLine();
        }

        private void ShowPlayerChoices(string answerID)
        {
            Debug.Log($"[ShowPlayerChoices] answerID={answerID} - 선택지 표시 시작");
            isInChoiceState = true;
            playerChoicePanel.SetActive(true);

            List<Dialogue> choiceArray = dialogueData.GetChoiceDialoguesByAnswerID(answerID);

            Debug.Log($"[ShowPlayerChoices] choiceArray.Count={choiceArray.Count}, choiceButtons.Length={choiceButtons.Length}");

            // 🔄 **플레이어 이름과 이미지 설정**
            if (choiceArray.Count > 0)
            {
                // 첫 번째 대사에서 플레이어 정보 가져옴
                playerNameText.text = choiceArray[0].charName; // 🔄 CSV에서 charName 가져오기

                if (!string.IsNullOrEmpty(choiceArray[0].charPortrait))
                {
                    Sprite playerSprite = Resources.Load<Sprite>(choiceArray[0].charPortrait); // 🔄 CSV에서 charPortrait로 이미지 로드
                    if (playerSprite)
                        playerPortraitImage.sprite = playerSprite;
                    else
                        Debug.LogWarning($"플레이어 이미지 {choiceArray[0].charPortrait}가 없습니다!");
                }
                else
                {
                    Debug.LogWarning("플레이어 이미지 정보가 없습니다!");
                }
            }

            // 🔄 모든 버튼 초기화
            foreach (var button in choiceButtons)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }

            // 🔄 선택지 표시
            for (int i = 0; i < choiceArray.Count; i++)
            {
                if (i >= choiceButtons.Length)
                {
                    Debug.LogWarning($"[ShowPlayerChoices] 선택지가 {choiceButtons.Length}개를 초과했습니다. 추가 버튼이 필요합니다.");
                    break;
                }

                choiceButtons[i].gameObject.SetActive(true);
                string lineText = choiceArray[i].lines.Length > 0 ? choiceArray[i].lines[0] : "";
                choiceButtonTexts[i].text = lineText;

                string nxt = choiceArray[i].nextLine;
                choiceButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"[ShowPlayerChoices] 선택지 클릭 - 다음 대화 {nxt}");
                    playerChoicePanel.SetActive(false);
                    isInChoiceState = false;
                    StartConversation(nxt, currentNPC);
                });
            }
        }




        private void EndConversation()
        {
            IsConversationActive = false;
            isConversationActive = false;
            currentDialogue = null;
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(false);
            isTyping = false;
            isInChoiceState = false;
        }
    }
}
