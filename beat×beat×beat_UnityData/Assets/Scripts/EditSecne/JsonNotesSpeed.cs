using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonNotesSpeed : MonoBehaviour
{
    [System.Serializable]
    public class NotesSpeedData
    {
        public float notesSpeed;
    }

    public static float ReadNotesSpeedJson()
    {
        NotesSpeedData myData = new NotesSpeedData();

        // ハイスコア読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/Config/NotesSpeedConfig.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<NotesSpeedData>(datastr); // ロードしたデータで上書き
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

        return myData.notesSpeed;
    }

    public static void WriteNotesSpeedJson(float notesSpeed)
    {
        NotesSpeedData myData = new NotesSpeedData();

        // ノーツスピード書き込み
        StreamWriter writer;
        myData.notesSpeed = notesSpeed;

        string jsonstr = JsonUtility.ToJson(myData);

        writer = new StreamWriter(Application.dataPath + "/Resources/Config/NotesSpeedConfig.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
}
