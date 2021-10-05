// (TimingMakerのほうのKeyCodeもGameUtilからとってきたほうがいいです．)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameUtil : MonoBehaviour
{
    public static KeyCode[] keyCode = new KeyCode[5];

    [System.Serializable]
    public class KeyConfig
    {
        // public KeyCodeClass[] notes;
        public KeyCode notes0;
        public KeyCode notes1;
        public KeyCode notes2;
        public KeyCode notes3;
        public KeyCode notes4;
    }

    /*
    [System.Serializable]
    public class KeyCodeClass
    {
        public KeyCode keyCode;
    }
    */

    void Start()
    {
        ReadKeyConfigJson();
    }

    public static void ReadKeyConfigJson()
    {
        KeyConfig myData = new KeyConfig();

        // ハイスコア読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            reader = new StreamReader(Application.dataPath + "/Resources/Config/KeyConfig.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<KeyConfig>(datastr); // ロードしたデータで上書き

            /*
            for (int i = 0; i < myData.notes.Length; i++)
            {
                keyCode[i] = myData.notes[i].keyCode;
                Debug.Log(keyCode[i]);
            }
            */
            keyCode[0] = myData.notes0;
            keyCode[1] = myData.notes1;
            keyCode[2] = myData.notes2;
            keyCode[3] = myData.notes3;
            keyCode[4] = myData.notes4;

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

    public static KeyCode GetKeyCodeByLineNum(int lineNum)
    {
        switch (lineNum)
        {
            case 0:
                return keyCode[0];
            case 1:
                return keyCode[1];
            case 2:
                return keyCode[2];
            case 3:
                return keyCode[3];
            case 4:
                return keyCode[4];
            default:
                return KeyCode.None;
        }
    }
}
