using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class SetGame : MonoBehaviour
{
    // 現在選択中のボタン
    private GameObject Button;
    // 現在の一つ前に選択していたボタン
    private GameObject ButtonBuf;
    private GameObject button;
    private Text buttonText;
    private GameObject Level;
    private Text LevelCountText;
    public static int levelCount;
    private Text BPMCountText;
    public static float _BPMCount;
    private GameObject highScore;
    private Text highScoreCountText;
    public static int highScoreCount;
    private Text optionNameText;
    public static int optionNum;
    private Text composerText;
    public static string composerName;

    private AudioSource audioSource;
    public static float previewStartTime;
    private bool _isPlaying;
    private bool delayFlag = false;

    public static string fileName;
    public static string songName;
    public static string audioFilePath;
    public static string notesFilePath;
    public static string scoreFilePath;

    void Start()
    {
        levelCount = 0;
        _BPMCount = 0;
        highScoreCount = 0;
        optionNum = 0;  // 0:正規、1:ミラー、2:ランダム、3:Rランダム、4:Sランダム
        _isPlaying = false;

        LevelCountText = GameObject.Find("Canvas/Level/Count").GetComponent<Text>();
        BPMCountText = GameObject.Find("Canvas/BPM/Count").GetComponent<Text>();
        composerText = GameObject.Find("Canvas/Composer/Name").GetComponent<Text>();
        highScoreCountText = GameObject.Find("Canvas/HighScore/Count").GetComponent<Text>();
        optionNameText = GameObject.Find("Canvas/NotesOption/OptionName").GetComponent<Text>();
        audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();

        fileName = "";
        songName = "";
        audioFilePath = "";
        notesFilePath = "";
        scoreFilePath = "";
    }

    void Update()
    {
        // 現在の選択されているボタンを取得し、Buttonに代入
        Button = EventSystem.current.currentSelectedGameObject;
        // Buttonがnullじゃない時（FadeIn、Outの時nullを出さないように）
        if (Button)
        {
            // 現在の選択されているボタンが前に選択されているボタンと違う場合(選択肢を移動させた場合)
            // 現在のボタンのエフェクトを表示させる
            // 一つ前に選択していたボタンのエフェクトを非表示にさせる
            if (Button != ButtonBuf)
            {
                // 選択中のボタンの一番上の子要素を取得
                // 普通のやり方が分からん
                int j = 0;
                foreach (Transform child in Button.transform)
                {
                    if (j == 0)
                    {
                        button = child.gameObject;
                        break;
                    }
                }
                buttonText = button.GetComponent<Text>();
                SetGameButton();
            }
            // 現在選択されているボタンをButtonBufに代入(現在のボタンを一つ前に選択していたボタンに設定する)
            ButtonBuf = Button;
        }

        // ループ処理（曲が終わったら再度再生）
        if (_isPlaying && !audioSource.isPlaying && !delayFlag) audioSource.Play();

        // optionNumに対応してオプション名を表示
        SetOption(optionNum);
    }

    void SetGameButton()
    {
        SelectButton script; // SelectButtonScriptが入る変数
        script = Button.GetComponent<SelectButton>(); // Buttonの中にあるSelectButtonScriptを取得して変数に格納する

        fileName = script._fileName;
        audioFilePath = fileName + "/Song";
        notesFilePath = fileName + "/Notes";
        scoreFilePath = fileName + "/Score";

        // jsonデータからコンポーザー、楽曲難易度、BPM、楽曲プレビュー用のpreviewStartTimeを取得
        ReadSongData();
        // ハイスコアを取得
        ReadScore();
        // 楽曲プレビュー
        LoadSong();
    }

    void ReadSongData()
    {
        JsonSongData.ReadSongJson(fileName);
        LevelCountText.text = levelCount.ToString();
        BPMCountText.text = _BPMCount.ToString();
        composerText.text = composerName;
    }

    void ReadScore()
    {
        JsonScorer.ReadScoreJson(fileName);
        highScoreCountText.text = highScoreCount.ToString();
    }

    void LoadSong()
    {
        // プレビューを再生、本当なら曲ごとの間に遅延を入れたい
        // AudioSource を取得
        audioSource.clip = Resources.Load(audioFilePath) as AudioClip;
        // 取得出来たらCSVデータをロード、出来なかったら選曲画面に戻る
        if (audioSource.clip)
        {
            audioSource.time = previewStartTime;
            audioSource.Play();
            _isPlaying = true;
        }
        else Debug.Log("No Name Song Data");

        // 曲が重ならないように遅延を入れる
        // StartCoroutine(DelayStartTime());
    }

    // optionNumに対応してオプション名を表示
    void SetOption(int num)
    {
        string optionName = "";
        if (num == 0) optionName = "Original";
        else if (num == 1) optionName = "Mirror";
        else if (num == 2) optionName = "Ramdom";
        else if (num == 3) optionName = "R-Random";
        else if (num == 4) optionName = "S-Random";
        optionNameText.text = optionName;
    }

    IEnumerator DelayStartTime()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
