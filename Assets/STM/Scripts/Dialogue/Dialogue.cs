using System;

namespace STM
{
    [Serializable]
    public class Dialogue
    {
        // CSV 파일 헤더에 해당하는 필드들
        public string lineID;          // 예: "mio_dialogue_01_01"
        public string dialogueType;    // "0" = NPC, "1" = 플레이어
        public string charID;       // SpeakerManager에서 관리하는 캐릭터의 고유 ID
        public string[] dialogueLines; // 해당 대화 블록의 모든 대사 라인
        public string nextDialogID;    // 다음 대화 블록의 ID (빈 값이면 대화 종료)

        public Dialogue(string lineID, string dialogueType, string charID, string[] dialogueLines, string nextDialogID)
        {
            this.lineID = lineID;
            this.dialogueType = dialogueType;
            this.charID = charID;
            this.dialogueLines = dialogueLines;
            this.nextDialogID = nextDialogID;
        }
    }
}