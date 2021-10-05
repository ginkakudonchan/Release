// 追加2、BPMに応じて微ずれを修正させる
// Burning Heart BPM142
// それは蜃気楼だった BPM210
// 月と狼 BPM158
// 煉獄セレナーデ BPM172
// シャイニングスター BPM158
// 雪の降る BPM118

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesTimingMaker : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _startTime = 0;   // 追加
    private CSVWriter _CSVWriter;   // 追加

    private bool _isPlaying = false; // 追加
    public GameObject startButton;

    // 追加2
    private static float BPM = 118;
    private static float offset = 0.6f; // スタートしてから曲が始まる時間
    private static float beat = 60 / (BPM * 4); // 16分刻みで補正

    void Start()
    {
        _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        _CSVWriter = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();    // 追加
    }

    void Update()
    {
        // 追加
        if (_isPlaying)
        {
            DetectKeys();
        }
    }

    public void StartMusic()
    {
        startButton.SetActive(false);
        _audioSource.Play();
        _startTime = Time.time; // 追加
        _isPlaying = true;   // 追加
    }

    // 追加
    void DetectKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WriteNotesTiming(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WriteNotesTiming(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            WriteNotesTiming(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            WriteNotesTiming(3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WriteNotesTiming(4);
        }
    }

    // 追加
    void WriteNotesTiming(int num)
    {
        Debug.Log(GetTiming());
        _CSVWriter.WriteCSV(GetTiming().ToString() + "," + num.ToString());
    }

    // 追加
    float GetTiming()
    {
        // 追加2
        float time = Time.time - _startTime - offset;
        int quotient = (int)(time / beat);  // quotient:商
        // return beat * quotient + offset;
        return Time.time - _startTime;
    }
}
