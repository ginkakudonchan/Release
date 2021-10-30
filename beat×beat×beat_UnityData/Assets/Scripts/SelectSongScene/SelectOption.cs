using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOption : MonoBehaviour
{
    void Update()
    {
        SetOption();
    }

    // F1~F5までのどれかを押したら、譜面オプションを変更
    void SetOption()
    {
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3) || Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.F5))
        {
            int num = 0;
            if (Input.GetKeyDown(KeyCode.F1)) num = 0;   // 正規
            else if (Input.GetKeyDown(KeyCode.F2)) num = 1;  // ミラー
            else if (Input.GetKeyDown(KeyCode.F3)) num = 2;  // ランダム
            else if (Input.GetKeyDown(KeyCode.F4)) num = 3;  // Rランダム
            else if (Input.GetKeyDown(KeyCode.F5)) num = 4;  // Sランダム
            SetGame.optionNum = num;
            // Debug.Log("optionNum = " + SetGame.optionNum);
        }
    }
}
