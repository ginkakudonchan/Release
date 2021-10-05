using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    private AudioSource _audioSource;   // 追加

    void Start()
    {
        _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();    // 追加    
    }


    [SerializeField]
    // ポーズした時に表示するUIのプレハブ
    private GameObject pauseUIPrefab;
    // ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                Time.timeScale = 0f;
                _audioSource.Pause();
            }
            else
            {
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
                _audioSource.UnPause();
            }
        }

        // ポーズ中でも曲を終了できるようにポーズを解除
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.R))
        {
            if (pauseUIInstance != null)
            {
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
        }
    }
}
