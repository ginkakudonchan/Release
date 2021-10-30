// 追加3 判定
// 追加4 エフェクト表示
// 追加5 static変数の初期化、シーンの切り替え

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private bool _isPlaying = false;
    private string songName;
    public GameObject[] notes;
    private float[] _timing = new float[1024];
    private static int[] _lineNum = new int[1024];

    private int _notesCount = 0;
    private int maxNotesNum = 0;

    public GameObject barLine;
    private float bpm;
    private float barTiming;
    private float barLineTiming;
    private float startBarLineTiming;
    private float endBarLineTiming;

    private AudioSource _audioSource;
    private float _startTime = 0;

    // オフセット、Earlyの時にプラス、Lateの時にマイナス方向に設定する
    public Text timeOffsetText;
    public static float timeOffset;
    public Text objectOffsetText;
    public static float objectOffset;

    public Text songText;
    public Text scoreText;
    public static int _score = 0;
    public static int _scoreEffectLimit;

    public Text comboText;
    public static int _combo = 0;
    public static int _comboEffectLimit;

    public Text notesSpeedNumText;

    public Text earlyText;
    public Text lateText;

    // ノーツタイミング
    // checkRange:判定チェックを始めるタイミング
    private float checkRange = 0.2f;
    private float goodRange = 0.133f;
    private float greatRange = 0.067f;
    private float perfectRange = 0.033f;

    // 基準に大して早遅のチェック
    private bool earlyFlag;
    private bool lateFlag;
    public GameObject panel;
    public Text firstText;
    public Text normalNotesText;
    public Text specialNotesText;
    public Text notesSpeedText;
    public Text judgeOffsetText;
    public Text judgeOffsetText2;
    public Text judgeOffsetText3;

    List<GameObject> NotesGameObject = new List<GameObject>();

    List<NotesScript_Tutorial> NotesNotesScript = new List<NotesScript_Tutorial>();
    List<BarLineScript_Tutorial> BarLines = new List<BarLineScript_Tutorial>();

    public static float notesSpeed;
    public static int nowNotesNum;

    [System.Serializable]
    public class NotesData
    {
        public string songName;
        public float bpm;
        public float startTime;
        public float endTime;
        public NotesClass[] notes;
    }

    [System.Serializable]
    public class NotesClass
    {
        public float timing;
        public int lineNum;
    }

    private bool firstGameFlag; // firstGameFlagが入る変数

    void Start()
    {
        firstGameFlag = JsonFirstGameFlag.ReadFirstGameFlagJson();

        // static変数の初期化
        _score = 0;
        _scoreEffectLimit = 10;
        _combo = 0;
        _comboEffectLimit = 10;
        nowNotesNum = 0;

        // AudioSource を取得
        _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        // 取得出来たらCSVデータをロード、出来なかったら選曲画面に戻る
        if (_audioSource.clip)
        {
            ReadNotesJson();
            ReadNotesSpeedJson();
            ReadOffsetJson();
            SpawnNotes_BarLine();
            StartGame();
        }
        else
        {
            Debug.Log("No Name Song Data");
            SceneManager.LoadScene("TitleScene");
        }
    }

    void Update()
    {
        if (_isPlaying)
        {
            // ポーズ中には何もしない処理
            if (Mathf.Approximately(Time.timeScale, 0f)) return;

            // 時間によって表示させるテキストを変化
            if (4.68f <= GetMusicTime() && GetMusicTime() <= 9.21f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(true);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (9.21f < GetMusicTime() && GetMusicTime() <= 15.98f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(true);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (15.98f < GetMusicTime() && GetMusicTime() <= 27.33f)
                panel.gameObject.SetActive(false);
            else if (27.33f < GetMusicTime() && GetMusicTime() <= 34.12f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(true);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (34.12f < GetMusicTime() && GetMusicTime() <= 45.44f)
                panel.gameObject.SetActive(false);
            else if (45.44f < GetMusicTime() && GetMusicTime() <= 54.50f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(true);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (54.50f < GetMusicTime() && GetMusicTime() <= 59.02f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(true);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (59.02f < GetMusicTime() && GetMusicTime() <= 65.8f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(true);
                judgeOffsetText3.gameObject.SetActive(false);
            }
            else if (65.82f < GetMusicTime() && GetMusicTime() <= 72.61f)
            {
                panel.gameObject.SetActive(true);
                firstText.gameObject.SetActive(false);
                normalNotesText.gameObject.SetActive(false);
                specialNotesText.gameObject.SetActive(false);
                notesSpeedText.gameObject.SetActive(false);
                judgeOffsetText.gameObject.SetActive(false);
                judgeOffsetText2.gameObject.SetActive(false);
                judgeOffsetText3.gameObject.SetActive(true);
            }
            else if (72.61f < GetMusicTime())
                panel.gameObject.SetActive(false);

            // スコアの更新
            scoreText.text = _score.ToString();

            // 10コンボ以上でコンボテキストを表示
            if (_combo >= 10) { comboText.gameObject.SetActive(true); comboText.text = _combo.ToString(); }
            else comboText.gameObject.SetActive(false);

            // 曲が終わったら
            if (!_audioSource.isPlaying)
            {
                _isPlaying = false;
                // 3秒後にタイトル画面
                StartCoroutine(WaitFullComboEffect());
            }

            // 始まって数秒はノーツが安定しないので応急処置
            if (GetMusicTime() < 5f) AdjustNotes_BarLine();

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
        while (barLineTiming <= endBarLineTiming)
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

        NotesScript_Tutorial n = Note.GetComponent<NotesScript_Tutorial>();
        NotesNotesScript.Add(n);
    }

    void SpawnBarLine()
    {
        GameObject BarLine;
        float positionY = ((barLineTiming + objectOffset / 1000f)) * notesSpeed;
        BarLine = Instantiate(barLine,
            new Vector3(0, positionY, 0),
            Quaternion.identity) as GameObject;

        BarLineScript_Tutorial b = BarLine.GetComponent<BarLineScript_Tutorial>();
        BarLines.Add(b);
    }

    void ReadNotesJson()
    {
        NotesData myData = new NotesData();

        // 譜面データ読み取り
        string datastr = "";
        StreamReader reader;

        try
        {
            songName = "チュートリアル";
            songText.text = songName;
            reader = new StreamReader(Application.dataPath + "/Resources/01_サイバー13/NotesData.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            myData = JsonUtility.FromJson<NotesData>(datastr); // ロードしたデータで上書き

            // BPMを取得
            bpm = myData.bpm;
            // 1小節の時間を計算
            barTiming = 60 * 4 / myData.bpm;
            // 最初の小節線の時間を取得
            startBarLineTiming = myData.startTime;
            barLineTiming = myData.startTime;
            // 最後の小節線の時間を取得
            endBarLineTiming = myData.endTime;
            int i = 0;
            for (i = 0; i < myData.notes.Length; i++)
            {
                _timing[i] = myData.notes[i].timing;
                _lineNum[i] = myData.notes[i].lineNum;
            }
            maxNotesNum = i;
        }
        // 例外処理、見つからないなら多分こっち
        catch (FileNotFoundException e)
        {
            Debug.Log("ファイル" + e.FileName + "が見つかりません。");
            ChangeScene_Tutorial.ChangeSceneKeyConfig();
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("ディレクトリが見つかりません。");
            ChangeScene_Tutorial.ChangeSceneKeyConfig();
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
        if (0 < timeOffset) timeOffsetText.text = "+" + (timeOffset / 100f).ToString("f2");
        else timeOffsetText.text = (timeOffset / 100f).ToString("f2");

        if (0 < objectOffset) objectOffsetText.text = "+" + (objectOffset / 100f).ToString("f2");
        else objectOffsetText.text = (objectOffset / 100f).ToString("f2");
    }

    float GetMusicTime()
    {
        return Time.time - _startTime;
    }

    void CheckInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(GameUtil.GetKeyCodeByLineNum(i))) CheckJudge(i, GetMusicTime() + timeOffset / 1000f);
        }
    }

    void CheckNotesSpeed()
    {
        // F1, F2を押している時はオフセットの調整で矢印キーを使用する
        if (!Input.GetKey(KeyCode.F1) && !Input.GetKey(KeyCode.F2))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) notesSpeed++;
                if (Input.GetKeyDown(KeyCode.RightArrow)) notesSpeed += 10.0f;
                if (Input.GetKeyDown(KeyCode.DownArrow)) notesSpeed--;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) notesSpeed -= 10.0f;

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
                if (Input.GetKeyDown(KeyCode.UpArrow)) timeOffset++;
                if (Input.GetKeyDown(KeyCode.RightArrow)) timeOffset += 10f;
                if (Input.GetKeyDown(KeyCode.DownArrow)) timeOffset--;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) timeOffset -= 10f;

                timeOffset = Mathf.Clamp(timeOffset, -500f, 500f);
                if (0 < timeOffset) timeOffsetText.text = "+" + (timeOffset / 100f).ToString("f2");
                else timeOffsetText.text = (timeOffset / 100f).ToString("f2");
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
                if (Input.GetKeyDown(KeyCode.UpArrow)) objectOffset++;
                if (Input.GetKeyDown(KeyCode.RightArrow)) objectOffset += 10f;
                if (Input.GetKeyDown(KeyCode.DownArrow)) objectOffset--;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) objectOffset -= 10f;
                objectOffset = Mathf.Clamp(objectOffset, -500f, 500f);
                if (0 < objectOffset) objectOffsetText.text = "+" + (objectOffset / 100f).ToString("f2");
                else objectOffsetText.text = (objectOffset / 100f).ToString("f2");

                AdjustNotes_BarLine();
            }
        }
    }

    void AdjustNotes_BarLine()
    {
        int _notesCount = 0;
        foreach (NotesScript_Tutorial n in NotesNotesScript)
        {
            n.ChangeNotesSpeed((_timing[_notesCount] + objectOffset / 1000f) - GetMusicTime(), notesSpeed);
            _notesCount++;
        }

        int barCount = 0;
        barLineTiming = startBarLineTiming;
        foreach (BarLineScript_Tutorial b in BarLines)
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
                if (minDiff == -1 || minDiff > diff) { minDiff = diff; minDiffIndex = i; }
            }
        }

        // EarlyかLateか判定
        if (minDiff != -1 && _timing[minDiffIndex] - timing >= 0) { earlyFlag = true; lateFlag = false; }
        else { earlyFlag = false; lateFlag = true; }

        // count;; 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
        // 空打ちじゃなければノーツを消去
        if (minDiff != -1 && minDiff < checkRange)
        {
            _timing[minDiffIndex] = -1;
            nowNotesNum++;
            NotesGameObject[minDiffIndex].gameObject.SetActive(false);

            if (minDiff < perfectRange) PerfectTimingFunc(num); // Debug.Log("Perfect!");
            else if (minDiff < greatRange)
            {
                if (earlyFlag) EarlyDisplay(minDiff);
                if (lateFlag) LateDisplay(minDiff);

                // Debug.Log("Great!");
                GreatTimingFunc(num);
            }
            else if (minDiff < goodRange)
            {
                if (earlyFlag) EarlyDisplay(minDiff);
                if (lateFlag) LateDisplay(minDiff);

                // Debug.Log("Good!");
                GoodTimingFunc(num);
            }
            else
            {
                if (earlyFlag) EarlyDisplay(minDiff);
                if (lateFlag) LateDisplay(minDiff);

                // Debug.Log("Miss!");
                MissTimingFunc(num);
            }
        }
        else AirTimingFunc(num);    // Debug.Log("through");
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
        earlyText.gameObject.SetActive(true);
        lateText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        earlyText.gameObject.SetActive(false);
    }

    IEnumerator LateText(float minDiff)
    {
        // lateTextと誤差を0.5秒表示、重ならないようにearlyTextは非表示
        lateText.gameObject.SetActive(true);
        earlyText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        lateText.gameObject.SetActive(false);
    }

    void PerfectTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 0);
        TouchEffectManager.Instance.PlayEffect(num, 0);
        _score += 3;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    void GreatTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 1);
        TouchEffectManager.Instance.PlayEffect(num, 1);
        _score += 2;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    void GoodTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 1);
        TouchEffectManager.Instance.PlayEffect(num, 1);
        _score++;
        _combo++;
        // スコアとコンボのエフェクト表示チェック
        CheckScore_Combo();
    }

    void MissTimingFunc(int num)
    {
        EffectManager.Instance.PlayEffect(num, 2);
        TouchEffectManager.Instance.PlayEffect(num, 2);
        _combo = 0;
    }

    void AirTimingFunc(int num)
    {
        TouchEffectManager.Instance.PlayEffect(num, 3);
    }

    private static void CheckScore_Combo()
    {
        if (_scoreEffectLimit == 10 && 10 <= _score) { ScoreEffect.Instance.PlayEffect(0); _scoreEffectLimit = 50; }
        else if (_scoreEffectLimit == 50 && 50 <= _score) { ScoreEffect.Instance.PlayEffect(1); _scoreEffectLimit = 100; }
        else if (_scoreEffectLimit == 100 && 100 <= _score) { ScoreEffect.Instance.PlayEffect(1); _scoreEffectLimit = 500; }
        else if (_scoreEffectLimit <= _score) { ScoreEffect.Instance.PlayEffect(2); _scoreEffectLimit += 500; }

        if (_comboEffectLimit == 10 && 10 <= _combo) { ComboEffect.Instance.PlayEffect(0); _comboEffectLimit = 50; }
        else if (_comboEffectLimit == 50 && 50 <= _combo) { ComboEffect.Instance.PlayEffect(1); _comboEffectLimit = 100; }
        else if (_comboEffectLimit <= _combo) { ComboEffect.Instance.PlayEffect(2); _comboEffectLimit += 100; }
    }
    void CheckFullCombo()
    {
        if (_combo == maxNotesNum) FullComboEffect.fullComboFlag = true;
    }

    IEnumerator WaitFullComboEffect()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        if (!firstGameFlag) ChangeScene_Tutorial.ChangeSceneKeyConfig();
        else ChangeScene_Tutorial.ChangeSceneTitle();
    }
}
