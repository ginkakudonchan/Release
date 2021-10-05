using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JsonScorer : MonoBehaviour
{

    [System.Serializable]
    public class ScoreData
    {
        public int score;
    }

    public static void WriteScoreJson(string fileName, int score)
    {
        ScoreData myData = new ScoreData();

        // ハイスコア書き込み
        StreamWriter writer;
        myData.score = score;

        string jsonstr = JsonUtility.ToJson(myData);

        writer = new StreamWriter(Application.dataPath + "/Resources/" + fileName + "/ScoreData.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public static void ReadScoreJson(string fileName)
    {
        ScoreData myData = new ScoreData();

        // ハイスコア読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/" + fileName + "/ScoreData.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<ScoreData>(datastr); // ロードしたデータで上書き
            SetGame.highScoreCount = myData.score;
        }
        // 例外処理、見つからないなら多分こっち
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイル" + e.FileName + "が見つかりません。");
            SetGame.highScoreCount = 0;
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("ディレクトリが見つかりません。");
            SetGame.highScoreCount = 0;
        }
    }
}
