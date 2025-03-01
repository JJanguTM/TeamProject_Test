using System;

namespace STM
{
    [Serializable]
    public class Dialogue
    {
        public string lineID;         // "LN010100"
        public string dialogueType;   // "0"=NPC, "1"=플레이어
        public string charName;       // 예: "미오"
        public string charPortrait;   // 예: "mio.png"
        public string[] lines;        // 한 lineID 안에 여러 줄
        public string nextLine;       // 다음 라인ID or "1" or "-1"

        public Dialogue(string lineID, string dialogueType, string charName,
                        string charPortrait, string[] lines, string nextLine)
        {
            this.lineID = lineID;
            this.dialogueType = dialogueType;
            this.charName = charName;
            this.charPortrait = charPortrait;
            this.lines = lines;
            this.nextLine = nextLine;
        }
    }
}
