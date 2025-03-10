using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Dialogue/Dialogue/SO", order = 1)]
public class DialogueSO : ScriptableObject
{
    [Header ("대화 블록 정보")]
    public string dialogueID;
    public string charID;

    [Header ("대사 내용")]
    [TextArea(3,10)]
    public string[] dialogueLines;

    [Header ("다음 대화 연결")]
    public string nextDialogID;
}
