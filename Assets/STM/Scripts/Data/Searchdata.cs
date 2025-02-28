using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewSearchObjectData", menuName = "SearchObject Data", order = 1)]
public class SearchObjectData : ScriptableObject
{
    public string objectName; // 오브젝트 이름
    [TextArea(3, 5)] public string description; // 오브젝트 설명
}

