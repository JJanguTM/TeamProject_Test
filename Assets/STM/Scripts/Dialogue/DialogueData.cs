using System;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class DialogueData : MonoBehaviour
    {
        // key: 대화 블록의 ID 또는 선택지 그룹 ID, value: 해당하는 Dialogue 객체들의 리스트
        private Dictionary<string, List<Dialogue>> dialogueDict = new Dictionary<string, List<Dialogue>>();

        private void Awake()
        {
            LoadAllDialogueFiles();
        }

        /// <summary>
        /// Resources/Dialogue 폴더 내의 모든 CSV 파일을 로드하여 파싱합니다.
        /// </summary>
        private void LoadAllDialogueFiles()
        {
            TextAsset[] dialogueFiles = Resources.LoadAll<TextAsset>("Dialogue");
            foreach (TextAsset file in dialogueFiles)
            {
                ParseCSV(file.text);
            }
        }

        /// <summary>
        /// 단일 CSV 파일의 텍스트를 파싱하여 Dialogue 객체를 생성한 후, 
        /// 해당 Dialogue의 lineID(또는 answer 그룹 ID)를 key로 dictionary에 추가합니다.
        /// </summary>
        private void ParseCSV(string csvText)
        {
            string[] rows = csvText.Split('\n');
            int i = 1; // 첫 번째 줄은 헤더
            while (i < rows.Length)
            {
                string row = rows[i];
                string[] cols = row.Split(',');

                // 최소 5개 컬럼 필요 및 첫 열에 값이 있어야 새 대화 블록의 시작으로 간주
                if (cols.Length < 5 || string.IsNullOrWhiteSpace(cols[0]))
                {
                    i++;
                    continue;
                }

                string lineID = cols[0].Trim();          // 대화 블록의 고유 식별자 또는 선택지 그룹 ID
                Debug.Log("Parsed key: '" + lineID + "'");
                string dialogueType = cols[1].Trim();      // "0"=NPC, "1"=플레이어 등
                string charID = cols[2].Trim();         // SpeakerManager에서 참조할 캐릭터 ID
                string firstLine = cols[3].Trim().Trim('"');
                string nextDialogID = cols[4].Trim();      // 다음 대화 블록 또는 선택지 그룹 ID (빈 값이면 대화 종료)

                List<string> dialogueLines = new List<string>();
                if (!string.IsNullOrEmpty(firstLine))
                    dialogueLines.Add(firstLine);

                i++;
                // 첫 열이 비어있는 행들은 같은 대화 블록에 속하는 추가 대사로 처리
                while (i < rows.Length)
                {
                    string[] nextCols = rows[i].Split(',');
                    if (nextCols.Length < 5)
                    {
                        i++;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(nextCols[0]))
                        break; // 새 대화 블록의 시작

                    string extraLine = nextCols[3].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(extraLine))
                        dialogueLines.Add(extraLine);

                    i++;
                }

                Dialogue dialogue = new Dialogue(lineID, dialogueType, charID, dialogueLines.ToArray(), nextDialogID);

                // 만약 같은 키(예: answer_01)로 이미 등록되어 있다면 리스트에 추가합니다.
                if (!dialogueDict.ContainsKey(lineID))
                {
                    dialogueDict[lineID] = new List<Dialogue>();
                }
                dialogueDict[lineID].Add(dialogue);
            }
        }

        /// <summary>
        /// 일반 대화의 경우, dialogID에 해당하는 첫 번째 Dialogue 객체를 반환합니다.
        /// (선택지 대화라면 여러 개가 있을 수 있으므로 GetChoiceDialoguesByAnswerID()를 사용)
        /// </summary>
        public Dialogue GetDialogueByID(string dialogID)
        {
            if (dialogueDict.TryGetValue(dialogID, out List<Dialogue> dialogues) && dialogues.Count > 0)
            {
                return dialogues[0];
            }
            Debug.LogError("대화 데이터를 찾을 수 없습니다: " + dialogID);
            return null;
        }

        /// <summary>
        /// 선택지 대화의 경우, answerID에 해당하는 모든 Dialogue 객체를 리스트로 반환합니다.
        /// </summary>
        public List<Dialogue> GetChoiceDialoguesByAnswerID(string answerID)
        {
            if (dialogueDict.TryGetValue(answerID, out List<Dialogue> dialogues))
            {
                return dialogues;
            }
            Debug.LogWarning("선택지 대화 데이터를 찾을 수 없습니다: " + answerID);
            return new List<Dialogue>();
        }
    }
}