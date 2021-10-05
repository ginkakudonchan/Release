using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Result : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeSceneTitle();
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeSceneSelectSong();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeSceneGame();
        }
    }

    public void ChangeSceneTitle()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Title());
    }

    public void ChangeSceneSelectSong()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_SelectSong());
    }

    public void ChangeSceneGame()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Game());
    }

    IEnumerator Wait_Title()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_SelectSong()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SelectSongScene");
    }

    IEnumerator Wait_Game()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameScene");
    }
}
