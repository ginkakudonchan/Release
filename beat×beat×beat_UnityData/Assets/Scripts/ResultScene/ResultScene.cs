using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    void Start()
    {
        // 元のスコアより高かった場合、ハイスコアをcsvに更新
        if (SetGame.highScoreCount < GameController._score)
        {
            SetGame.highScoreCount = GameController._score;
            string fileName = GameController.fileName;
            int score = GameController._score;
            JsonScorer.WriteScoreJson(fileName, score);
        }
    }
}
