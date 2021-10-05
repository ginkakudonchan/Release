using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_SelectSong : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeSceneTitle();
        }
    }

    public void ChangeSceneTitle()
    {
        SetGame.songName = "";
        SetGame.audioFilePath = "";
        SetGame.notesFilePath = "";
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Title());
    }

    public void ChangeSceneGame()
    {
        if (SetGame.songName == ""
            || SetGame.audioFilePath == ""
            || SetGame.notesFilePath == "")
        {
            Debug.Log("No Data! Select Song!");
        }
        else if (TitleScene.gameSceneFlag)
        {
            GameController.fileName = SetGame.fileName;
            GameController.songName = SetGame.songName;
            GameController.audioFilePath = SetGame.audioFilePath;
            GameController.notesFilePath = SetGame.notesFilePath;
            SetGame.songName = "";
            SetGame.audioFilePath = "";
            SetGame.notesFilePath = "";
            FadeScript.fadeOutFlag = true;
            StartCoroutine(Wait_Game());
        }
        else if (TitleScene.editSceneFlag)
        {
            EditController.songName = SetGame.songName;
            EditController.audioFilePath = SetGame.audioFilePath;
            EditController.notesFilePath = SetGame.notesFilePath;
            SetGame.songName = "";
            SetGame.audioFilePath = "";
            SetGame.notesFilePath = "";
            FadeScript.fadeOutFlag = true;
            StartCoroutine(Wait_Edit());
        }
    }

    IEnumerator Wait_Title()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_Game()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator Wait_Edit()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("EditScene");
    }
}
