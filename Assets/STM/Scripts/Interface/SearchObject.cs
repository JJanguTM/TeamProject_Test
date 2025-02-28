using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{
    [SerializeField] private GameObject takeALookUI; // ✅ UI 오브젝트 (Canvas 내 `TakeALook`)
    private bool isPlayerInRange = false; // ✅ 플레이어가 상호작용 범위 안에 있는지 확인

    private void Start()
    {
        if (takeALookUI != null)
        {
            takeALookUI.SetActive(false); // 게임 시작 시 UI 비활성화
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F)) // ✅ 상호작용 키 (F)를 누르면
        {
            ToggleUI(); // ✅ UI 활성화/비활성화
        }
    }

    private void ToggleUI()
    {
        if (takeALookUI != null)
        {
            bool isActive = takeALookUI.activeSelf;
            takeALookUI.SetActive(!isActive); // 현재 상태 반전
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ✅ 플레이어가 SearchObject 범위에 들어옴
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ✅ 플레이어가 범위를 벗어나면
        {
            isPlayerInRange = false;
            if (takeALookUI != null)
            {
                takeALookUI.SetActive(false); // ✅ 자동으로 UI 숨김
            }
        }
    }
}