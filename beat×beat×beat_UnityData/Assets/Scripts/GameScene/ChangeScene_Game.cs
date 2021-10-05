using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Game : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Wait_Title());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Wait_SelectSong());
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Wait_Game());
        }
    }

    void ChangeSceneTitle()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Title());
    }

    public static void ChangeSceneSelectSong()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        SceneManager.LoadScene("SelectSongScene");
    }

    public static void ChangeSceneGame()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        SceneManager.LoadScene("GameScene");
    }

    public static void ChangeSceneResult()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        SceneManager.LoadScene("ResultScene");
    }

    IEnumerator Wait_Title()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_SelectSong()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SelectSongScene");
    }
    IEnumerator Wait_Game()
    {
        GameController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(GameController.notesSpeed);
        JsonOffset.WriteOffsetJson(GameController.timeOffset, GameController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameScene");
    }
}
