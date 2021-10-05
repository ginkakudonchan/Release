using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFirstGameFlag : MonoBehaviour
{
    [System.Serializable]
    public class FirstGameFlagData
    {
        public bool firstGameFlag;
    }

    public static bool ReadFirstGameFlagJson()
    {
        FirstGameFlagData myData = new FirstGameFlagData();

        // ハイスコア読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/Config/FirstGameFlag.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<FirstGameFlagData>(datastr); // ロードしたデータで上書き
        }
        // 例外処理、見つからないなら多分こっち
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイル" + e.FileName + "が見つかりません。");
            myData.firstGameFlag = false;
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("ディレクトリが見つかりません。");
            myData.firstGameFlag = false;
        }

        return myData.firstGameFlag;
    }

    public static void WriteFirstGameFlagJson(bool firstGameFlag)
    {
        FirstGameFlagData myData = new FirstGameFlagData();

        // ノーツスピード書き込み
        StreamWriter writer;
        myData.firstGameFlag = firstGameFlag;
        string jsonstr = JsonUtility.ToJson(myData);

        writer = new StreamWriter(Application.dataPath + "/Resources/Config/FirstGameFlag.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
}
