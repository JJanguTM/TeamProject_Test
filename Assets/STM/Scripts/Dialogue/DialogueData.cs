using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    public class DialogueData : MonoBehaviour
    {
        [SerializeField] private string csvFileName;

        private Dialogue[] dialogues;
        private Dictionary<string, Dialogue> dialogueDict;

        private void Start()
        {
            dialogues = ParseCSV(csvFileName);

            dialogueDict = new Dictionary<string, Dialogue>();
            foreach (Dialogue D in dialogues)
            {
                if (!dialogueDict.ContainsKey(D.LineID))
                    dialogueDict.Add(D.LineID, D);
            }
        }

        private Dialogue[] ParseCSV(string csvFileName)
        {
            TextAsset csvData = Resources.Load<TextAsset>(csvFileName);
            if (csvData == null)
            {
                Debug.LogError($"CSV 파일이 없습니다: {csvFileName}");
                return Array.Empty<Dialogue>();
            }

            string[] rows = csvData.text.Split('\n');
           
            int dialogueCount = 0;
            for (int i = 1; i < rows.Length; i++)
            {
                string[] cols = rows[i].Split(',');
                if (cols.Length >= 5 && !string.IsNullOrWhiteSpace(cols[0]))
                {
                    dialogueCount++;
                }
            }

            Dialogue[] result = new Dialogue[dialogueCount];
            int index = 0;

            for (int i = 1; i < rows.Length;)
            {
                string[] cols = rows[i].Split(',');

                if (cols.Length < 5 || string.IsNullOrWhiteSpace(cols[0]))
                {
                    i++;
                    continue;
                }

                string lineID = cols[0].Trim();
                string dialogueType = cols[1].Trim();
                string charName = cols[2].Trim();
                string nextLine = cols[4].Trim();

                List<string> lines = new List<string>();

                do
                {
                   
                    if (!string.IsNullOrWhiteSpace(cols[3]))
                    {
                        lines.Add(cols[3].Trim());
                    }

                    i++;
                    if (i < rows.Length)
                    {
                        cols = rows[i].Split(',');
                    }
                    else break;
                }
                while (string.IsNullOrWhiteSpace(cols[0])); 

                Dialogue D = new Dialogue(
                    lineID,
                    dialogueType,
                    charName,
                    lines.ToArray(),
                    nextLine
                );

                result[index++] = D;
            }

            return result;
        }

        public Dialogue[] GetAllDialogues()
        {
            return dialogues;
        }

        public Dialogue GetDialogueById(string lineID)
        {
            if (dialogueDict.TryGetValue(lineID, out Dialogue D))
                return D;

            return null;
        }

        public List<Dialogue> GetAnswersFrom(string questionLineID)
        {
            List<Dialogue> answerList = new List<Dialogue>();

            bool questionFound = false;
            for (int i = 0; i < dialogues.Length; i++)
            {
                if (dialogues[i].LineID == questionLineID)
                {
                    questionFound = true;
                    continue;
                }

                if (questionFound)
                {
                    if (dialogues[i].DialogueType == "answer")
                    {
                   
                        answerList.Add(dialogues[i]);
                    }
                    else if (dialogues[i].DialogueType == "normal")
                    {
                   
                    break;
                    }
                }
            }

            return answerList;
        }
    }
}
