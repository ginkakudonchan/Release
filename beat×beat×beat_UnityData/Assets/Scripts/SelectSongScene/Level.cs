using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Level : MonoBehaviour
{
    public Text LevelCountText;
    public static int levelCount = 0;
    public static int _BPM = 0;

    void Start()
    {
        LevelCountText.text = levelCount.ToString();
    }

    /*
    void LoadCSV()
    {
        TextAsset csv = Resources.Load(notesFilePath) as TextAsset;
        if (csv)
        {
            StringReader reader = new StringReader(csv.text);

            // 1行目、曲の難易度読み取り
            string line = reader.ReadLine();
            Level.LevelCount = int.Parse(line);

        }
        else
        {
            Debug.Log("No Name Notes Data");
            ChangeScene_Game.ChangeSceneSelectSong();
        }
    }
    */
}
