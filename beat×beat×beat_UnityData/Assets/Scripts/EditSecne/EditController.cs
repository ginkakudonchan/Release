// 【Unity】Instantiateで生成したGameObjectのScriptに引数を渡す方法(https://qiita.com/2dgames_jp/items/495dc59c78930e284707)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class EditController : MonoBehaviour
{
    public static bool _isPlaying;
    public static string songName;

    public GameObject[] notes;
    public static float[] _timing;
    public static int[] _lineNum;

    public static string notesFilePath;
    private int _notesCount = 0;
    public static float _startTime;

    // オフセット、Earlyの時にプラス、Lateの時にマイナス方向に設定する
    public Text timeOffsetText;
    public static float timeOffset;
    public Text objectOffsetText;
    public static float objectOffset;

    public GameObject barLine;
    private float bpm;
    private int bar;
    public static float barTiming;
    private float barLineTiming;
    public static float startBarLineTiming;
    public static int endBarLineNum;

    public Text barLineNumText;
    public Text notesSpeedNumText;
    public Text songText;
    public Text startText;

    private AudioSource _audioSource;
    public static string audioFilePath;

    List<NotesScript> Notes = new List<NotesScript>();
    List<BarLineScript> BarLines = new List<BarLineScript>();

    public static float notesSpeed;

    void Start()
    {
        if (TitleScene.editSceneFlag)
        {
            // Pause中にリセットしても不具合無いように初期化
            Time.timeScale = 1f;

            // static変数の初期化
            _isPlaying = false;
            _timing = new float[2048];
            _lineNum = new int[2048];
            _startTime = 0;
            bar = 0;
            barTiming = 0;
            endBarLineNum = 0;

            // AudioSource を取得
            _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            _audioSource.clip = Resources.Load(audioFilePath) as AudioClip;
            // 取得出来たらCSVデータをロード、出来なかったら選曲画面に戻る
            if (_audioSource.clip)
            {
                bpm = SetGame._BPMCount;
                // 1小節の時間を計算
                barTiming = 60 * 4 / bpm;

                songText.text = songName;
                LoadCSV();
                ReadNotesSpeedJson();
                ReadOffsetJson();
                SpawnNotes_BarLine();
            }
            else
            {
                Debug.Log("No Name Song Data");
                StartCoroutine(Wait_SelectSong());
            }
        }
        else
        {
            Debug.Log("No Data! Select Mode!");
            StartCoroutine(Wait_Title());
        }
    }

    void Update()
    {
        if (!_isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && bar < endBarLineNum)
            {
                bar++;
                barLineNumText.text = bar.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && bar < endBarLineNum)
            {
                if (endBarLineNum < bar + 10)
                {
                    bar = endBarLineNum;
                }
                else
                {
                    bar += 10;
                }

                barLineNumText.text = bar.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && 0 < bar)
            {
                bar--;
                barLineNumText.text = bar.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && 0 < bar)
            {
                if (bar - 10 < 0)
                {
                    bar = 0;
                }
                else
                {
                    bar -= 10;
                }
                barLineNumText.text = bar.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
        else
        {
            // Rが押されるか曲が終わったら開始前に戻る
            if (Input.GetKeyDown(KeyCode.R) || !_audioSource.isPlaying)
            {
                RestartEdit();
            }

            // キー入力
            CheckInput();
            // ノーツスピード変更チェック
            CheckNotesSpeed();
            // 時間オフセットの変更チェック
            CheckTimeOffset();
            // オブジェクトオフセットの変更チェック
            CheckObjectOffset();
        }
    }

    public void StartGame()
    {
        _isPlaying = true;
        _startTime = Time.time;
        startText.gameObject.SetActive(false);

        if (0 == bar)
        {
            _audioSource.time = 0;
        }
        else if (0 < bar)
        {
            _audioSource.time = (bar - 1) * barTiming + startBarLineTiming;
        }
        _audioSource.Play();
    }

    void SpawnNotes_BarLine()
    {
        // ノーツ生成
        while (_timing[_notesCount] != 0)
        {
            SpawnNotes(_lineNum[_notesCount]);
            _notesCount++;
        }

        // 小節線生成
        int barCount = 0;
        while (barCount < endBarLineNum)
        {
            SpawnBarLine();
            barCount++;
            // 毎回barTimingを足すよりもこの方が精度が良いかも？
            barLineTiming = barCount * barTiming + startBarLineTiming;
        }
        barLineTiming = startBarLineTiming;
    }

    void SpawnNotes(int num)
    {
        float positionX = 0.0f;
        float positionY = (_timing[_notesCount] + objectOffset / 1000f) * notesSpeed;
        if (0 <= num && num <= 3)
        {
            positionX = -6.0f + (4.0f * num);
        }

        GameObject Note;
        Note = Instantiate(notes[num],
            new Vector3(positionX, positionY, 0),
            Quaternion.identity) as GameObject;

        NotesScript n = Note.GetComponent<NotesScript>();
        Notes.Add(n);
    }

    void SpawnBarLine()
    {
        GameObject BarLine;
        float positionY = (barLineTiming + objectOffset / 1000f) * notesSpeed;
        BarLine = Instantiate(barLine,
            new Vector3(0, positionY, 0),
            Quaternion.identity) as GameObject;

        BarLineScript b = BarLine.GetComponent<BarLineScript>();
        BarLines.Add(b);
    }

    void LoadCSV()
    {
        TextAsset csv = Resources.Load(notesFilePath) as TextAsset;
        if (csv)
        {
            StringReader reader = new StringReader(csv.text);

            // 1行目の読み取り
            // startTime、endTime
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            // 最初の小節線の時間を取得
            startBarLineTiming = float.Parse(values[0]);
            barLineTiming = startBarLineTiming;
            // 全小節線の数を取得
            endBarLineNum = int.Parse(values[1]);

            // 2行目以降、譜面データ読み取り
            int i = 0;
            while (reader.Peek() > -1)
            {
                line = reader.ReadLine();
                values = line.Split(',');

                // 同時押しが最大5個まで可能だから、5個分のノーツデータを読み込めるようにする
                for (int loop = 1; loop < Math.Min(6, values.Length); loop++)
                {
                    // データが空でなく、整数の時のみノーツデータとしてみなす
                    var result = 0;
                    var ret = int.TryParse(values[loop], out result);   // 整数の時true、小数や文字の時falseを返す
                    if (values[loop] != "" && ret == true)
                    {
                        _timing[i] = float.Parse(values[0]);
                        _lineNum[i] = int.Parse(values[loop]);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No Name Notes Data");
            StartCoroutine(Wait_SelectSong());
        }
    }

    void ReadNotesSpeedJson()
    {
        notesSpeed = JsonNotesSpeed.ReadNotesSpeedJson();
        notesSpeedNumText.text = (notesSpeed / 10f).ToString("f1");
    }

    public static float GetMusicTime()
    {
        return Time.time - _startTime;
    }

    void ReadOffsetJson()
    {
        (timeOffset, objectOffset) = JsonOffset.ReadOffsetJson();
        if (0 < timeOffset)
        {
            timeOffsetText.text = "+" + (timeOffset / 100f).ToString("f2");
        }
        else
        {
            timeOffsetText.text = (timeOffset / 100f).ToString("f2");
        }

        if (0 < objectOffset)
        {
            objectOffsetText.text = "+" + (objectOffset / 100f).ToString("f2");
        }
        else
        {
            objectOffsetText.text = (objectOffset / 100f).ToString("f2");
        }
    }

    void CheckInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(GameUtil.GetKeyCodeByLineNum(i)))
            {
                TouchEffectManager.Instance.PlayEffect(i, 3);
            }
        }
    }

    void CheckNotesSpeed()
    {
        // F1, F2を押している時はオフセットの調整で矢印キーを使用する
        if (!Input.GetKey(KeyCode.F1) && !Input.GetKey(KeyCode.F2))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    notesSpeed++;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    notesSpeed += 10.0f;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    notesSpeed--;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    notesSpeed -= 10.0f;
                }

                // Mathf.Clamp: Unity独自の関数？
                // 最小値と最大値を超えないようにできる
                notesSpeed = Mathf.Clamp(notesSpeed, 5.0f, 100.0f);
                notesSpeedNumText.text = (notesSpeed / 10.0f).ToString("f1");

                int _notesCount = 0;
                foreach (NotesScript n in Notes)
                {
                    n.ChangeNotesSpeed((_timing[_notesCount] + objectOffset / 1000f) - (GetMusicTime() + (bar - 1) * barTiming + startBarLineTiming), notesSpeed);
                    _notesCount++;
                }

                int barCount = 0;
                barLineTiming = startBarLineTiming;
                foreach (BarLineScript b in BarLines)
                {
                    b.ChangeNotesSpeed((barLineTiming + objectOffset / 1000f) - (GetMusicTime() + (bar - 1) * barTiming + startBarLineTiming), notesSpeed);
                    barCount++;
                    barLineTiming = barCount * barTiming + startBarLineTiming;
                }
            }
        }
    }

    void CheckTimeOffset()
    {
        // F1を押している時は時間オフセットの調整で矢印キーを使用する
        if (Input.GetKey(KeyCode.F1) && !Input.GetKey(KeyCode.F2))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    timeOffset++;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    timeOffset += 10f;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    timeOffset--;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    timeOffset -= 10f;
                }

                timeOffset = Mathf.Clamp(timeOffset, -500f, 500f);
                if (0 < timeOffset)
                {
                    timeOffsetText.text = "+" + (timeOffset / 100f).ToString("f2");
                }
                else
                {
                    timeOffsetText.text = (timeOffset / 100f).ToString("f2");
                }
            }
        }
    }

    void CheckObjectOffset()
    {
        // F2を押している時はオブジェクトオフセットの調整で矢印キーを使用する
        if (!Input.GetKey(KeyCode.F1) && Input.GetKey(KeyCode.F2))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    objectOffset++;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    objectOffset += 10f;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    objectOffset--;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    objectOffset -= 10f;
                }

                objectOffset = Mathf.Clamp(objectOffset, -500f, 500f);
                if (0 < objectOffset)
                {
                    objectOffsetText.text = "+" + (objectOffset / 100f).ToString("f2");
                }
                else
                {
                    objectOffsetText.text = (objectOffset / 100f).ToString("f2");
                }

                int _notesCount = 0;
                foreach (NotesScript n in Notes)
                {
                    n.ChangeNotesSpeed((_timing[_notesCount] + objectOffset / 1000f) - (GetMusicTime() + (bar - 1) * barTiming + startBarLineTiming), notesSpeed);
                    _notesCount++;
                }

                int barCount = 0;
                barLineTiming = startBarLineTiming;
                foreach (BarLineScript b in BarLines)
                {
                    b.ChangeNotesSpeed((barLineTiming + objectOffset / 1000f) - (GetMusicTime() + (bar - 1) * barTiming + startBarLineTiming), notesSpeed);
                    barCount++;
                    barLineTiming = barCount * barTiming + startBarLineTiming;
                }
            }
        }
    }

    void RestartEdit()
    {
        _isPlaying = false;
        startText.gameObject.SetActive(true);
        _audioSource.Stop();

        int _notesCount = 0;
        foreach (NotesScript n in Notes)
        {
            n.RestartEdit(_timing[_notesCount] + objectOffset / 1000f, bar, startBarLineTiming, barTiming, notesSpeed);
            _notesCount++;
        }

        int barCount = 0;
        barLineTiming = startBarLineTiming;
        foreach (BarLineScript b in BarLines)
        {
            b.RestartEdit(barLineTiming + objectOffset / 1000f, bar, startBarLineTiming, barTiming, notesSpeed);
            barCount++;
            barLineTiming = barCount * barTiming + startBarLineTiming;
        }
    }

    IEnumerator Wait_Title()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        ChangeSecne_Edit.ChangeSceneTitle();
    }

    IEnumerator Wait_SelectSong()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        ChangeSecne_Edit.ChangeSceneSelectSong();
    }
}
