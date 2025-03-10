using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace STM
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Speaker & Dialogue Settings")]
        [SerializeField] private SpeakerManager speakerManager;   // Speaker 정보를 조회할 매니저
        [SerializeField] private DialogueData dialogueData;       // CSV 파일로부터 대화 데이터를 로드하는 컴포넌트
        [SerializeField] private string startDialogID;              // 시작 대화 블록 ID
        [SerializeField] private bool playOnAwake = true;           // 자동 시작 여부

        [Header("NPC Dialogue UI")]
        [SerializeField] private GameObject npcPanel;
        [SerializeField] private Text npcNameText;
        [SerializeField] private Text npcDialogueText;
        [SerializeField] private Image npcPortraitImage;

        [Header("Player Choice UI")]
        [SerializeField] private GameObject playerChoicePanel;
        [SerializeField] private Text playerNameText;
        [SerializeField] private Image playerPortraitImage;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private Text[] choiceButtonTexts;

        [Header("Typing Settings")]
        [SerializeField] private float typingSpeed = 0.05f;

        // 현재 진행 중인 대화 데이터
        private Dialogue currentDialogue;
        private int currentLineIndex = 0;
        public bool isConversationActive { get; private set; } = false;

        // 타이핑 효과 관련 변수
        private bool isTyping = false;
        private float typingTimer = 0f;
        private int charIndex = 0;
        private string fullText = "";

        private void Start()
        {
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(false);

            if (playOnAwake)
            {
                StartConversation(startDialogID);
            }
        }

        /// <summary>
        /// 지정한 dialogID를 기반으로 대화를 시작합니다.
        /// </summary>
        public void StartConversation(string dialogID)
        {
            // 플레이어 선택지 UI가 활성화되어 있다면 숨깁니다.
            playerChoicePanel.SetActive(false);

            currentDialogue = dialogueData.GetDialogueByID(dialogID);
            if (currentDialogue == null)
            {
                EndConversation();
                return;
            }
            isConversationActive = true;
            currentLineIndex = 0;
            ShowCurrentLine();

            Debug.Log("nextDialogID: '" + currentDialogue.nextDialogID + "'");
        }

        /// <summary>
        /// 현재 대화 블록의 대사 라인을 타이핑 효과와 함께 표시합니다.
        /// 모든 라인 출력 후 nextDialogID에 따라 다음 동작을 결정합니다.
        /// </summary>
        private void ShowCurrentLine()
        {
            npcPanel.SetActive(true);
            playerChoicePanel.SetActive(false);

            // SpeakerManager를 통해 스피커 정보를 조회합니다.
            var speaker = speakerManager.GetSpeakerByID(currentDialogue.charID);
            if (speaker != null)
            {
                npcNameText.text = speaker.charName;
                if (speaker.charPortrait != null)
                {
                    npcPortraitImage.sprite = speaker.charPortrait;
                }
                else
                {
                    Debug.LogWarning("Speaker 초상화가 없습니다. ID: " + currentDialogue.charID);
                }
            }
            else
            {
                npcNameText.text = "알 수 없음";
            }

            // 현재 대화 블록의 모든 대사 라인이 출력되었으면 다음 단계를 처리합니다.
            if (currentLineIndex >= currentDialogue.dialogueLines.Length)
            {
                if (string.IsNullOrEmpty(currentDialogue.nextDialogID))
                {
                    EndConversation();
                    Debug.Log("??");
                }
                else if (currentDialogue.nextDialogID.StartsWith("answer"))
                {
                    // nextDialogID가 "answer" 패턴이면 플레이어 선택지를 표시합니다.
                    ShowPlayerChoices(currentDialogue.nextDialogID);
                }
                else
                {
                    StartConversation(currentDialogue.nextDialogID);
                }
                return;
            }

            fullText = currentDialogue.dialogueLines[currentLineIndex];
            npcDialogueText.text = "";
            charIndex = 0;
            typingTimer = 0f;
            isTyping = true;
        }

        private void Update()
        {
            if (!isConversationActive)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndConversation();
                return;
            }

            // 타이핑 효과 구현
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

            // Enter 키 입력: 타이핑 중이면 전체 텍스트 표시, 아니면 다음 대사로 진행
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isTyping)
                {
                    isTyping = false;
                    npcDialogueText.text = fullText;
                }
                else
                {
                    currentLineIndex++;
                    ShowCurrentLine();
                }
            }
        }

        /// <summary>
        /// 플레이어 선택지를 표시합니다.
        /// answerID를 기반으로 DialogueData에서 선택지 대화 목록을 가져옵니다.
        /// </summary>
        private void ShowPlayerChoices(string answerID)
        {
            // CSV 기반 대화 데이터에서 선택지 대사 목록을 가져오는 메서드가 필요합니다.
            // (예: DialogueData.GetChoiceDialoguesByAnswerID(answerID))
            List<Dialogue> choices = dialogueData.GetChoiceDialoguesByAnswerID(answerID);
            if (choices == null || choices.Count == 0)
            {
                Debug.LogWarning("선택지 대화가 없습니다. answerID: " + answerID);
                EndConversation();
                return;
            }

            isConversationActive = false; // 대화를 일시 중지합니다.
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(true);

            // 플레이어 UI에 첫 번째 선택지 대사의 스피커 정보를 적용합니다.
            var speaker = speakerManager.GetSpeakerByID(choices[0].charID);
            if (speaker != null)
            {
                playerNameText.text = speaker.charName;
                if (speaker.charPortrait != null)
                {
                    playerPortraitImage.sprite = speaker.charPortrait;
                }
            }
            else
            {
                playerNameText.text = "알 수 없음";
            }

            // 기존 선택지 버튼 초기화
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].gameObject.SetActive(false);
                choiceButtons[i].onClick.RemoveAllListeners();
            }

            // 각 선택지 버튼에 선택지 대화 내용을 할당합니다.
            for (int i = 0; i < choices.Count && i < choiceButtons.Length; i++)
            {
                choiceButtons[i].gameObject.SetActive(true);
                string choiceText = choices[i].dialogueLines.Length > 0 ? choices[i].dialogueLines[0] : "";
                choiceButtonTexts[i].text = choiceText;

                string nextDialog = choices[i].nextDialogID;
                choiceButtons[i].onClick.AddListener(() =>
                {
                    playerChoicePanel.SetActive(false);
                    StartConversation(nextDialog);
                });
            }
        }

        /// <summary>
        /// 대화를 종료하고 모든 UI를 숨깁니다.
        /// </summary>
        private void EndConversation()
        {
            isConversationActive = false;
            npcPanel.SetActive(false);
            playerChoicePanel.SetActive(false);
            Debug.Log("Dialogue ended, but why?");
        }

        // NPCInteract와의 호환성을 위한 메서드 (필요시)
        public Dialogue GetDialogueByID(string dialogID)
        {
            return dialogueData.GetDialogueByID(dialogID);
        }
    }
}