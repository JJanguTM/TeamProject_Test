using System.Collections.Generic;
using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    // Speaker 데이터가 담긴 CSV 파일 (첫 줄은 헤더)
    [SerializeField] private TextAsset speakerDataFile;
    
    // Speaker 정보를 저장하는 Dictionary (키: charID)
    private Dictionary<string, Speaker> speakerDict;

    private void Awake()
    {
        LoadSpeakers();
    }

    /// <summary>
    /// CSV 파일에서 Speaker 데이터를 로드하여 Dictionary에 저장합니다.
    /// CSV 파일은 각 행이 charID, charName, portraitFileName 형식이어야 합니다.
    /// </summary>
    private void LoadSpeakers()
    {
        speakerDict = new Dictionary<string, Speaker>();

        if (speakerDataFile == null)
        {
            Debug.LogError("Speaker 데이터 파일이 지정되지 않았습니다!");
            return;
        }

        // 줄 단위로 분할 (첫 줄은 헤더)
        string[] rows = speakerDataFile.text.Split('\n');
        for (int i = 1; i < rows.Length; i++)
        {
            string row = rows[i].Trim();
            if (string.IsNullOrEmpty(row))
                continue;

            // 쉼표로 분할하여 각 컬럼을 읽어옵니다.
            string[] cols = row.Split(',');
            if (cols.Length < 3)
            {
                Debug.LogWarning($"Speaker 데이터 형식 오류: {row}");
                continue;
            }

            string charID = cols[0].Trim();
            string charName = cols[1].Trim();
            string portraitFileName = cols[2].Trim();

            // Portrait는 Resources 폴더 내 "Portraits" 하위 폴더에서 로드합니다.
            Sprite portrait = Resources.Load<Sprite>("Portraits/" + portraitFileName);
            if (portrait == null)
            {
                Debug.LogWarning($"Speaker {charName} (ID: {charID})의 초상화 '{portraitFileName}'를 찾을 수 없습니다.");
            }

            // Speaker 객체 생성 후 Dictionary에 추가
            if (!speakerDict.ContainsKey(charID))
            {
                Speaker speaker = new Speaker(charID, charName, portrait);
                speakerDict.Add(charID, speaker);
            }
            else
            {
                Debug.LogWarning($"중복된 Speaker ID가 발견되었습니다: {charID}");
            }
        }
    }

    /// <summary>
    /// 지정된 speakerID에 해당하는 Speaker 객체를 반환합니다.
    /// </summary>
    /// <param name="speakerID">Speaker 고유 식별자</param>
    /// <returns>찾은 Speaker 객체 또는 없을 경우 null</returns>
    public Speaker GetSpeakerByID(string speakerID)
    {
        if (speakerDict != null && speakerDict.TryGetValue(speakerID, out Speaker speaker))
        {
            return speaker;
        }
        Debug.LogWarning($"Speaker 정보를 찾을 수 없습니다. ID: {speakerID}");
        return null;
    }
}