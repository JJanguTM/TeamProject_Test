using System;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class DialogueData : MonoBehaviour
    {
        [SerializeField] private TextAsset csvFile;
        private Dictionary<string, List<Dialogue>> dialogueDict;  // 중복 허용을 위한 Dictionary

        private void Awake()
        {
            dialogueDict = new Dictionary<string, List<Dialogue>>();
            ParseCSV();
        }

        private void ParseCSV()
        {
            if (csvFile == null)
            {
                Debug.LogError("CSV 파일이 지정되지 않았습니다!");
                return;
            }


            string[] rows = csvFile.text.Split('\n');
            int i = 1; // 0번줄은 헤더
            while (i < rows.Length)
            {
                string row = rows[i];
                string[] cols = row.Split(',');

                if (cols.Length < 6 || string.IsNullOrWhiteSpace(cols[0]))
                {
                    i++;
                    continue;
                }

                string lineID = cols[0].Trim();
                string dialogueType = cols[1].Trim();
                string charName = cols[2].Trim();
                string charPortrait = cols[3].Trim();
                string firstLine = cols[4].Trim().Trim('"');
                string nextLine = cols[5].Trim();

                List<string> lines = new List<string>();
                if (!string.IsNullOrEmpty(firstLine))
                    lines.Add(firstLine);

                i++;
                while (i < rows.Length)
                {
                    string[] nextCols = rows[i].Split(',');
                    if (nextCols.Length < 6)
                    {
                        i++;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(nextCols[0]))
                        break;

                    string extraLine = nextCols[4].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(extraLine))
                        lines.Add(extraLine);

                    i++;
                }

                Dialogue d = new Dialogue(lineID, dialogueType, charName, charPortrait, lines.ToArray(), nextLine);
                if (!dialogueDict.ContainsKey(lineID))
                {
                    dialogueDict[lineID] = new List<Dialogue>();
                }
                dialogueDict[lineID].Add(d);

   
            }
        }

        // ✅ 첫 번째 대사 반환하도록 수정!
        public Dialogue GetDialogueByID(string lineID)
        {
            if (dialogueDict.TryGetValue(lineID, out List<Dialogue> dialogues) && dialogues.Count > 0)
            {
                return dialogues[0];  // 🔄 첫 번째 대사 반환
            }
        
            return null;
        }

        // ✅ 선택지 대사들 반환
        public List<Dialogue> GetChoiceDialoguesByAnswerID(string answerID)
        {
            List<Dialogue> choiceDialogues = new List<Dialogue>();
            if (dialogueDict.TryGetValue(answerID, out List<Dialogue> dialogues))
            {
                foreach (Dialogue d in dialogues)
                {
                    choiceDialogues.Add(d);
         
                }
            }
            else
            {
             
            }

        
            return choiceDialogues;
        }
    }
}
