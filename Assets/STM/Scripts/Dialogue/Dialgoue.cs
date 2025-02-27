using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STM
{
    [Serializable]
    public class Dialogue
    {
        public string LineID { get; private set;}     
        public string DialogueType { get; private set; }    
        public string Name { get; private set; }           
        public string[] Lines { get; private set; }
        public string NextLine { get; private set; }

        public Dialogue(string lineID, string dialogueType, string name, string[] lines, string nextLine)
        {
            LineID = lineID;
            DialogueType  = dialogueType;
            Name = name;
            Lines = lines;
            NextLine = nextLine;
        }
    }
}
