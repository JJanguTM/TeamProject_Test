using UnityEngine;
using UnityEngine.UI;

public class SearchObject : MonoBehaviour
{
    [SerializeField] private GameObject takeALookUI; // ✅ UI 오브젝트 (Canvas 내 `TakeALook`)
    [SerializeField] private Text descriptionText; // ✅ UI 내 Text 컴포넌트
    [SerializeField] private SearchObjectData searchData; // ✅ 스크립터블 오브젝트 (각각의 설명 데이터)

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (takeALookUI != null)
        {
            takeALookUI.SetActive(false); // 게임 시작 시 UI 비활성화
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleUI(); // ✅ UI 활성화/비활성화
        }
    }

    private void ToggleUI()
    {
        if (takeALookUI != null)
        {
            bool isActive = takeALookUI.activeSelf;
            takeALookUI.SetActive(!isActive); // ✅ UI 활성화/비활성화

            // ✅ UI가 활성화될 때만 설명 텍스트 업데이트
            if (!isActive && searchData != null && descriptionText != null)
            {
                descriptionText.text = searchData.description;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (takeALookUI != null)
            {
                takeALookUI.SetActive(false);
            }
        }
    }
}
