using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // ここに注意

public class CSVScoreWriter : MonoBehaviour
{
    // ハイスコアをCSVに書き込む処理
    public static void WriteCSV(string songName, string txt)
    {
        StreamWriter streamWriter;
        FileInfo fileInfo;
        string fileName = Application.dataPath + "/Resources/" + songName + "/Score.csv";
        fileInfo = new FileInfo(fileName);
        streamWriter = new StreamWriter(fileName);
        streamWriter.WriteLine(txt);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
