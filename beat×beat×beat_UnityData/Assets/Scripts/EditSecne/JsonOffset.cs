using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonOffset : MonoBehaviour
{
    [System.Serializable]
    public class OffsetData
    {
        public float timeOffset;
        public float objectOffset;
    }

    public static (float, float) ReadOffsetJson()
    {
        OffsetData myData = new OffsetData();

        // ハイスコア読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/Config/OffsetConfig.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<OffsetData>(datastr); // ロードしたデータで上書き
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

        return (myData.timeOffset, myData.objectOffset);
    }

    public static void WriteOffsetJson(float timeOffset, float objectOffset)
    {
        OffsetData myData = new OffsetData();

        // ノーツスピード書き込み
        StreamWriter writer;
        myData.timeOffset = timeOffset;
        myData.objectOffset = objectOffset;

        string jsonstr = JsonUtility.ToJson(myData);

        writer = new StreamWriter(Application.dataPath + "/Resources/Config/OffsetConfig.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
}
