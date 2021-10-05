using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeDisplay : MonoBehaviour
{
    public Text songText;

    // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
    public Text[] CountText;
    public static int[] count;

    void Start()
    {
        // static変数の初期化
        songText.text = GameController.songName;
        count = new int[CountText.Length];
    }

    void Update()
    {
        // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
        for (int i = 0; i < count.Length; i++)
        {
            CountText[i].text = count[i].ToString();
        }
    }
}
