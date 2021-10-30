using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeDisplay : MonoBehaviour
{
    public Text songText;
    public Text optionNameText;

    // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
    public Text[] CountText;
    public static int[] count;

    void Start()
    {
        // static変数の初期化
        songText.text = GameController.songName;
        count = new int[CountText.Length];

        // SetGame.optionNumに対応してオプション名を表示
        SetOption(SetGame.optionNum);
    }

    void Update()
    {
        // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
        for (int i = 0; i < count.Length; i++) CountText[i].text = count[i].ToString();
    }

    // SetGame.optionNumに対応してオプション名を表示
    void SetOption(int num)
    {
        string optionName = "";
        if (num == 0) optionName = "Original";
        else if (num == 1) optionName = "Mirror";
        else if (num == 2) optionName = "Ramdom";
        else if (num == 3) optionName = "R-Random";
        else if (num == 4) optionName = "S-Random";
        optionNameText.text = optionName;
    }
}
