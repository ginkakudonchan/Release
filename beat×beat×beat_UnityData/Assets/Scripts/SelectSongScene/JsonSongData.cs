using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSongData : MonoBehaviour
{
    [System.Serializable]
    public class SongData
    {
        public string songName;
        public string composer;
        public int level;
        public float _BPM;
        public float previewStartTime;
    }

    public static void ReadSongJson(string songName)
    {
        SongData myData = new SongData();

        // 楽曲データ読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/" + songName + "/SongData.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<SongData>(datastr); // ロードしたデータで上書き
            SetGame.songName = myData.songName;
            SetGame.composerName = myData.composer;
            SetGame.levelCount = myData.level;
            SetGame._BPMCount = myData._BPM;
            SetGame.previewStartTime = myData.previewStartTime;
        }
        // 例外処理、見つからないなら多分こっち
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイル" + e.FileName + "が見つかりません。");
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("ディレクトリが見つかりません。");
        }
    }

    public static string ReadSongNameJson(string songName)
    {
        SongData myData = new SongData();

        // 楽曲データ読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/" + songName + "/SongData.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<SongData>(datastr); // ロードしたデータで上書き
            return myData.songName;
        }
        // 例外処理、見つからないなら多分こっち
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイル" + e.FileName + "が見つかりません。");
            return "";
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("ディレクトリが見つかりません。");
            return "";
        }
    }
}
