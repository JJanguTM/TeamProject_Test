using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleActiveState(bool state)
    {
        gameObject.SetActive(state);
        Debug.Log($"{gameObject.name}가 {(state ? "활성화" : "비활성화")}되었습니다!");
    }
}
