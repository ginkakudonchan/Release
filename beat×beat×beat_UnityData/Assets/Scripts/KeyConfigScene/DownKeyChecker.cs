// Unity - 入力されたキーの情報を取得する
// https://qiita.com/pilkul/items/6351a967372541d92718

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;   // enum型を使うために必要
using System.IO;
using UnityEngine.SceneManagement;

public class DownKeyChecker : MonoBehaviour
{
    private KeyCode[] keyCode = new KeyCode[5];

    [System.Serializable]
    public class KeyConfig
    {
        public KeyCode notes0;
        public KeyCode notes1;
        public KeyCode notes2;
        public KeyCode notes3;
        public KeyCode notes4;
    }

    private bool _isPlaying = false;
    private bool _setting = false;
    public Text describeText;
    public Text beforeSettingText;
    public Text beforeSettingValueText;
    public Text afterSettingText;
    public Text afterSettingValueText;
    public Text settingUpText;
    public Text settingCompletedText;
    public Text warningText1;
    public Text warningText2;

    int i = 0;

    public TitleScene titleScrene; // firstGameFlagが入る変数

    void Start()
    {
        JsonFirstGameFlag.WriteFirstGameFlagJson(true);
        StartKeyConfig();
        ReadKeyConfigJson();
        Debug.Log("キーコンフィグ、入力どうぞ");
    }

    void Update()
    {
        if (_isPlaying)
        {
            DownKeyCheck();
        }
    }

    void StartKeyConfig()
    {
        _isPlaying = true;
    }

    void DownKeyCheck()
    {
        if (!_setting && i < 5)
        {
            if (i != 4)
            {
                describeText.text = "ノーマルノーツ" + (i + 1) + "で使用するキーを入力してください";
            }
            else
            {
                describeText.text = "スペシャルノーツで使用するキーを入力してください";
            }

            beforeSettingValueText.text = keyCode[i].ToString();
            afterSettingValueText.text = "";

            // 何かキーが押されたら
            if (Input.anyKeyDown)
            {
                // foreach文はある配列やリストに格納されている要素一つ一つに一回ずつアクセスすることのできる文です。
                // 今回の場合はcodeという変数にKeyCodeを一つずつ取り出して代入してきます。
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        // 元々使用しているキーは使用できない仕様
                        if (code == KeyCode.Q || code == KeyCode.R || code == KeyCode.P || code == KeyCode.UpArrow || code == KeyCode.DownArrow || code == KeyCode.RightArrow || code == KeyCode.LeftArrow)
                        {
                            // 設定処理
                            WarningKeyCode1(code);
                            break;
                        }
                        // 押したらTitleかOptionに戻るので何もしない
                        else if (code == KeyCode.Escape)
                        {
                            break;
                        }
                        // 既に設定したキーと競合しているか確認
                        else if (i != 0)
                        {
                            bool flag = false;
                            for (int j = 0; j < i; j++)
                            {
                                if (code == keyCode[j])
                                {
                                    flag = true;
                                    // 設定処理
                                    WarningKeyCode2(code);
                                    break;
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                        }

                        // 上のどれにも当てはまらなければ設定できる
                        // 設定処理
                        SettingKeyCode(code);
                        break;
                    }
                }
            }
        }
        else if (!_setting && i == 5)
        {
            WriteScoreJson();
            StartCoroutine(WaitSettingCompleted());
            // i++;
        }
    }

    void ReadKeyConfigJson()
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

    void WriteScoreJson()
    {
        KeyConfig myData = new KeyConfig();

        // ハイスコア書き込み
        StreamWriter writer;
        /*
        for (int i = 0; i < 5; i++)
        {
            myData.notes[i].keyCode = keyCode[i];
        }
        */
        myData.notes0 = keyCode[0];
        myData.notes1 = keyCode[1];
        myData.notes2 = keyCode[2];
        myData.notes3 = keyCode[3];
        myData.notes4 = keyCode[4];

        string jsonstr = JsonUtility.ToJson(myData);

        writer = new StreamWriter(Application.dataPath + "/Resources/Config/KeyConfig.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    void WarningKeyCode1(KeyCode code)
    {
        Debug.Log("そのキーは使用できません");
        Debug.Log(code);
        StartCoroutine(WaitWarning1(code));

    }

    IEnumerator WaitWarning1(KeyCode code)
    {
        _setting = true;
        // 設定中テキストを1秒間表示
        afterSettingValueText.text = code.ToString();
        warningText1.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        warningText1.gameObject.SetActive(false);
        _setting = false;
    }

    void WarningKeyCode2(KeyCode code)
    {

        Debug.Log("他のキーと競合しています");
        Debug.Log(code);
        StartCoroutine(WaitWarning2(code));

    }

    IEnumerator WaitWarning2(KeyCode code)
    {
        _setting = true;
        // 警告テキストを1秒間表示
        afterSettingValueText.text = code.ToString();
        warningText2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        warningText2.gameObject.SetActive(false);
        _setting = false;
    }

    void SettingKeyCode(KeyCode code)
    {
        Debug.Log(i + "番目");
        Debug.Log(code);
        StartCoroutine(WaitSettingUp(code));

    }

    IEnumerator WaitSettingUp(KeyCode code)
    {
        _setting = true;
        keyCode[i] = code;
        // 設定中テキストを1秒間表示
        afterSettingValueText.text = code.ToString();
        settingUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        settingUpText.gameObject.SetActive(false);
        _setting = false;
        i++;
    }

    IEnumerator WaitSettingCompleted()
    {
        Debug.Log("データを保存しました");
        describeText.gameObject.SetActive(false);
        beforeSettingText.gameObject.SetActive(false);
        beforeSettingValueText.gameObject.SetActive(false);
        afterSettingText.gameObject.SetActive(false);
        afterSettingValueText.gameObject.SetActive(false);
        settingCompletedText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(Wait_Title());

        /*
        describeText.gameObject.SetActive(false);
        beforeSettingText.gameObject.SetActive(false);
        beforeSettingValueText.gameObject.SetActive(false);
        afterSettingText.gameObject.SetActive(false);
        afterSettingValueText.gameObject.SetActive(false);
        settingCompletedText.gameObject.SetActive(false);
        */
    }

    IEnumerator Wait_Title()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("TitleScene");
    }
}
