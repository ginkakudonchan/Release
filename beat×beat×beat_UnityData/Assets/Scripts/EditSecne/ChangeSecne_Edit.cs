using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSecne_Edit : MonoBehaviour
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
    }

    public static void ChangeSceneTitle()
    {
        EditController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(EditController.notesSpeed);
        JsonOffset.WriteOffsetJson(EditController.timeOffset, EditController.objectOffset);
        SceneManager.LoadScene("TitleScene");
    }

    public static void ChangeSceneSelectSong()
    {
        EditController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(EditController.notesSpeed);
        JsonOffset.WriteOffsetJson(EditController.timeOffset, EditController.objectOffset);
        SceneManager.LoadScene("SelectSongScene");
    }

    IEnumerator Wait_Title()
    {
        EditController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(EditController.notesSpeed);
        JsonOffset.WriteOffsetJson(EditController.timeOffset, EditController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_SelectSong()
    {
        EditController._isPlaying = false;
        JsonNotesSpeed.WriteNotesSpeedJson(EditController.notesSpeed);
        JsonOffset.WriteOffsetJson(EditController.timeOffset, EditController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SelectSongScene");
    }
}
