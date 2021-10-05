// 追加3 判定
// 追加4 エフェクト表示
// 追加5 static変数の初期化、シーンの切り替え

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static bool _isPlaying;
    public static string fileName;
    public static string songName;

    public GameObject[] notes;
    public static float[] _timing;
    public static int[] _lineNum;

    public static string notesFilePath;
    private int _notesCount = 0;
    private int maxNotesNum;

    public GameObject barLine;
    private float bpm;
    // private int bar = 0;
    private float barTiming;
    private float barLineTiming;
    private float startBarLineTiming;
    private int endBarLineNum = 0;

    private AudioSource _audioSource;
    public static string audioFilePath;
    public static float _startTime;

    // オフセット、Earlyの時にプラス、Lateの時にマイナス方向に設定する
    public Text timeOffsetText;
    public static float timeOffset;
    public Text objectOffsetText;
    public static float objectOffset;

    public Text scoreText;
    public static int _score;
    public static int _scoreEffectLimit;

    public Text comboText;
    public static int _combo;
    public static int _comboEffectLimit;
    public Text maxComboText;
    public static int _maxCombo;

    public Text notesSpeedNumText;

    public Text earlyText;
    public Text earlyTimeText;
    public Text lateText;
    public Text lateTimeText;

    // ノーツタイミング
    // checkRange:判定チェックを始めるタイミング
    float checkRange = 0.2f;
    float goodRange = 0.133f;
    float greatRange = 0.067f;
    float perfectRange = 0.033f;

    // 基準に大して早遅のチェック
    bool earlyFlag;
    bool lateFlag;

    public static int nowNotesNum;

    List<GameObject> NotesGameObject = new List<GameObject>();

    List<NotesScript> NotesNotesScript = new List<NotesScript>();
    List<BarLineScript> BarLines = new List<BarLineScript>();

    public static float notesSpeed = 70.0f;

    void Start()
    {
        if (TitleScene.gameSceneFlag)
        {
            // Pause中にリセットしても不具合無いように初期化
            Time.timeScale = 1f;

            // static変数の初期化
            _isPlaying = false;
            _timing = new float[2048];
            _lineNum = new int[2048];
            _startTime = 0;
            _score = 0;
            _scoreEffectLimit = 10;
            _combo = 0;
            _comboEffectLimit = 10;
            _maxCombo = 0;
            nowNotesNum = 0;
            notesSpeed = 30.0f;

            // AudioSource を取得
            _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            _audioSource.clip = Resources.Load(audioFilePath) as AudioClip;
            // 取得出来たらCSVデータをロード、出来なかったら選曲画面に戻る
            if (_audioSource.clip)
            {
                bpm = SetGame._BPMCount;
                // 1小節の時間を計算
                barTiming = 60 * 4 / bpm;
                LoadCSV();
                ReadNotesSpeedJson();
                ReadOffsetJson();
                SpawnNotes_BarLine();
                StartGame();
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
            StartCoroutine(Wait_Game());
        }
    }

    void Update()
    {
        if (_isPlaying)
        {
            // ポーズ中には何もしない処理
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                return;
            }

            scoreText.text = _score.ToString();
            _maxCombo = Math.Max(_maxCombo, _combo);
            maxComboText.text = _maxCombo.ToString();

            if (_combo >= 10)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = _combo.ToString();
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }

            // 曲が終わったら
            if (!_audioSource.isPlaying)
            {
                _isPlaying = false;
                // 3秒後にリザルト画面
                StartCoroutine(WaitFullComboEffect());
            }

            // 始まって数秒はノーツが安定しないので応急処置
            if (GetMusicTime() < 5f)
            {
                AdjustNotes_BarLine();
            }

            // キー入力 
            CheckInput();
            // ノーツスピード変更チェック
            CheckNotesSpeed();
            // 時間オフセットの変更チェック
            CheckTimeOffset();
            // オブジェクトオフセットの変更チェック
            CheckObjectOffset();
            // フルコンボチェック
            CheckFullCombo();
        }
    }

    public void StartGame()
    {
        _isPlaying = true;
        _startTime = Time.time;
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

    // ノーツの生成、生成後の位置は、ずれているからAdjustNotes_BarLineで調整
    void SpawnNotes(int num)
    {
        float positionX = 0.0f;
        float positionY = ((_timing[_notesCount] + objectOffset / 1000f)) * notesSpeed;
        if (0 <= num && num <= 3)
        {
            positionX = -6.0f + (4.0f * num);
        }

        GameObject Note;
        Note = Instantiate(notes[num],
            new Vector3(positionX, positionY, 0),
            Quaternion.identity) as GameObject;

        NotesGameObject.Add(Note);

        NotesScript n = Note.GetComponent<NotesScript>();
        NotesNotesScript.Add(n);
    }

    // 小節線の生成、生成後の位置は、ずれているからAdjustNotes_BarLineで調整
    void SpawnBarLine()
    {
        GameObject BarLine;
        float positionY = ((barLineTiming + objectOffset / 1000f)) * notesSpeed;
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

            /*
            BPM変化に対応できるようにしたい
            int k = 2;
            int kk = 0;
            float[] barLineNum = new float[100];
            float[] _bpm = new float[100];
            while (values[kk] != null && values[kk + 1] != null)
            {
                barLineNum[kk] = float.Parse(values[k]);
                _bpm[kk] = float.Parse(values[k + 1]);

                kk++;
                k += 2;
            }
            */

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
            // 最大コンボ数を取得
            maxNotesNum = i;
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
        notesSpeedNumText.text = (notesSpeed / 10.0f).ToString("f1");
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

    public static float GetMusicTime()
    {
        return Time.time - _startTime;
    }

    void CheckInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(GameUtil.GetKeyCodeByLineNum(i)))
            {
                CheckJudge(i, GetMusicTime() + timeOffset / 1000f);
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

                AdjustNotes_BarLine();
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

                AdjustNotes_BarLine();
            }
        }
    }

    void AdjustNotes_BarLine()
    {
        int _notesCount = 0;
        foreach (NotesScript n in NotesNotesScript)
        {
            n.ChangeNotesSpeed((_timing[_notesCount] + objectOffset / 1000f) - GetMusicTime(), notesSpeed);
            _notesCount++;
        }

        int barCount = 0;
        barLineTiming = startBarLineTiming;
        foreach (BarLineScript b in BarLines)
        {
            b.ChangeNotesSpeed((barLineTiming + objectOffset / 1000f) - GetMusicTime(), notesSpeed);
            barCount++;
            barLineTiming = barCount * barTiming + startBarLineTiming;
        }
    }

    void CheckJudge(int num, float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        for (int i = 0; i < _timing.Length; i++)
        {
            if (_timing[i] > 0 && _lineNum[i] == num)
            {
                float diff = Math.Abs(_timing[i] - timing);
                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        // EarlyかLateか判定
        if (minDiff != -1 && _timing[minDiffIndex] - timing >= 0)
        {
            earlyFlag = true;
            lateFlag = false;
        }
        else
        {
            earlyFlag = false;
            lateFlag = true;
        }

        // count;; 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
        // 空打ちじゃなければノーツを消去
        if (minDiff != -1 && minDiff < checkRange)
        {
            _timing[minDiffIndex] = -1;
            nowNotesNum++;
            NotesGameObject[minDiffIndex].gameObject.SetActive(false);

            if (minDiff < perfectRange)
            {
                JudgeDisplay.count[3]++;

                // Debug.Log("Perfect!");
                PerfectTimingFunc(num);
            }
            else if (minDiff < greatRange)
            {
                if (earlyFlag)
                {
                    JudgeDisplay.count[2]++;
                    EarlyDisplay(minDiff);
                }
                if (lateFlag)
                {
                    JudgeDisplay.count[4]++;
                    LateDisplay(minDiff);
                }

                // Debug.Log("Great!");
                GreatTimingFunc(num);
            }
            else if (minDiff < goodRange)
            {
                if (earlyFlag)
                {
                    JudgeDisplay.count[1]++;
                    EarlyDisplay(minDiff);
                }
                if (lateFlag)
                {
                    JudgeDisplay.count[5]++;
                    LateDisplay(minDiff);
                }

                // Debug.Log("Good!");
                GoodTimingFunc(num);
            }
            else
            {
                if (earlyFlag)
                {
                    JudgeDisplay.count[0]++;
                    EarlyDisplay(minDiff);
                }
                if (lateFlag)
                {
                    JudgeDisplay.count[6]++;
                    LateDisplay(minDiff);
                }

                // Debug.Log("Miss!");
                MissTimingFunc(num);
            }
        }
        else
        {
            // Debug.Log("through");
            AirTimingFunc(num);
        }
    }

    void EarlyDisplay(float minDiff)
    {
        StartCoroutine(EarlyText(minDiff));
    }

    void LateDisplay(float minDiff)
    {
        StartCoroutine(LateText(minDiff));
    }

    IEnumerator EarlyText(float minDiff)
    {
        // earlyTextと誤差を0.5秒表示、重ならないようにlateTextは非表示
        earlyTimeText.text = "+" + minDiff.ToString("f4");
        earlyText.gameObject.SetActive(true);
        earlyTimeText.gameObject.SetActive(true);
        lateText.gameObject.SetActive(false);
        lateTimeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        earlyText.gameObject.SetActive(false);
        earlyTimeText.gameObject.SetActive(false);
    }

    IEnumerator LateText(float minDiff)
    {
        // lateTextと誤差を0.5秒表示、重ならないようにearlyTextは非表示
        lateTimeText.text = "-" + minDiff.ToString("f4");
        lateText.gameObject.SetActive(true);
        lateTimeText.gameObject.SetActive(true);
        earlyText.gameObject.SetActive(false);
        earlyTimeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        lateText.gameObject.SetActive(false);
        lateTimeText.gameObject.SetActive(false);
    }

    public static void PerfectTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 0);
        TouchEffectManager.Instance.PlayEffect(num, 0);
        _score += 3;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    public static void GreatTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 1);
        TouchEffectManager.Instance.PlayEffect(num, 1);
        _score += 2;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    public static void GoodTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 1);
        TouchEffectManager.Instance.PlayEffect(num, 1);
        _score++;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    public static void MissTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 2);
        TouchEffectManager.Instance.PlayEffect(num, 2);
        _combo = 0;
        _comboEffectLimit = 10;
    }

    public static void AirTimingFunc(int num)
    {
        TouchEffectManager.Instance.PlayEffect(num, 3);
    }

    private static void CheckScore_Combo()
    {
        if (_scoreEffectLimit == 10 && 10 <= _score)
        {
            ScoreEffect.Instance.PlayEffect(0);
            _scoreEffectLimit = 50;
        }
        else if (_scoreEffectLimit == 50 && 50 <= _score)
        {
            ScoreEffect.Instance.PlayEffect(1);
            _scoreEffectLimit = 100;
        }
        else if (_scoreEffectLimit == 100 && 100 <= _score)
        {
            ScoreEffect.Instance.PlayEffect(1);
            _scoreEffectLimit = 500;
        }
        else if (_scoreEffectLimit <= _score)
        {
            ScoreEffect.Instance.PlayEffect(2);
            _scoreEffectLimit += 500;
        }

        if (_comboEffectLimit == 10 && 10 <= _combo)
        {
            ComboEffect.Instance.PlayEffect(0);
            _comboEffectLimit = 50;
        }
        else if (_comboEffectLimit == 50 && 50 <= _combo)
        {
            ComboEffect.Instance.PlayEffect(1);
            _comboEffectLimit = 100;
        }
        else if (_comboEffectLimit <= _combo)
        {
            ComboEffect.Instance.PlayEffect(2);
            _comboEffectLimit += 100;
        }
    }
    void CheckFullCombo()
    {
        if (_combo == maxNotesNum)
        {
            FullComboEffect.fullComboFlag = true;
        }
    }

    IEnumerator WaitFullComboEffect()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Wait_Result());
    }

    IEnumerator Wait_SelectSong()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        ChangeScene_Game.ChangeSceneSelectSong();
    }

    IEnumerator Wait_Game()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        ChangeScene_Game.ChangeSceneGame();
    }

    IEnumerator Wait_Result()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        ChangeScene_Game.ChangeSceneResult();
    }
}