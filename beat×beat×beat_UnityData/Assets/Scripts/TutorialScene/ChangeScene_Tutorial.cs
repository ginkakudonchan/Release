using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Tutorial : MonoBehaviour
{
    private bool firstGameFlag; // firstGameFlagが入る変数

    void Start()
    {
        firstGameFlag = JsonFirstGameFlag.ReadFirstGameFlagJson();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!firstGameFlag)
            {
                StartCoroutine(Wait_KeyConfig());
            }
            else
            {
                StartCoroutine(Wait_Title());
            }
        }
    }

    public static void ChangeSceneKeyConfig()
    {
        JsonNotesSpeed.WriteNotesSpeedJson(TutorialController.notesSpeed);
        JsonOffset.WriteOffsetJson(TutorialController.timeOffset, TutorialController.objectOffset);
        SceneManager.LoadScene("KeyConfigScene");
    }

    public static void ChangeSceneTitle()
    {
        JsonNotesSpeed.WriteNotesSpeedJson(TutorialController.notesSpeed);
        JsonOffset.WriteOffsetJson(TutorialController.timeOffset, TutorialController.objectOffset);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_KeyConfig()
    {
        JsonNotesSpeed.WriteNotesSpeedJson(TutorialController.notesSpeed);
        JsonOffset.WriteOffsetJson(TutorialController.timeOffset, TutorialController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("KeyConfigScene");
    }

    IEnumerator Wait_Title()
    {
        JsonNotesSpeed.WriteNotesSpeedJson(TutorialController.notesSpeed);
        JsonOffset.WriteOffsetJson(TutorialController.timeOffset, TutorialController.objectOffset);
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
