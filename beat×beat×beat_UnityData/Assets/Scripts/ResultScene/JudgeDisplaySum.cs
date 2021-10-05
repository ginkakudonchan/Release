using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeDisplaySum : MonoBehaviour
{
    public Text songText;
    public Text scoreText;
    public Text maxCouboText;

    // 0: perfectSum, 1: greatSum, 2: goodSum, 3: missSum
    public Text[] countText;
    private int[] sumCount;

    void Start()
    {
        sumCount = new int[countText.Length];

        // 0=3, 1=2+4, 2=1+5, 3=0+6
        int countHalf = sumCount.Length - 1;
        sumCount[0] = JudgeDisplay.count[countHalf];
        for (int i = 1; i < sumCount.Length; i++)
        {
            sumCount[i] += JudgeDisplay.count[countHalf - i];
            sumCount[i] += JudgeDisplay.count[countHalf + i];
        }

        songText.text = GameController.songName;
        scoreText.text = GameController._score.ToString();
        maxCouboText.text = GameController._maxCombo.ToString();

        for (int i = 0; i < sumCount.Length; i++)
        {
            countText[i].text = sumCount[i].ToString();
        }
    }
}
