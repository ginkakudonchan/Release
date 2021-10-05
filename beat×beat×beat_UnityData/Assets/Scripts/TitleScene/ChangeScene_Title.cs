using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Title : MonoBehaviour
{
    public void ChangeSceneGame()
    {
        TitleScene.gameSceneFlag = true;
        TitleScene.editSceneFlag = false;
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Game());
    }

    public void ChangeSceneEdit()
    {
        TitleScene.gameSceneFlag = false;
        TitleScene.editSceneFlag = true;
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Edit());
    }

    public void ChangeSceneTutorial()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Tutorial());
    }

    public void ChangeSceneOption()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Option());
    }

    public void ChangeSceneKeyConfig()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_KeyConfig());
    }

    IEnumerator Wait_Game()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SelectSongScene");
    }

    IEnumerator Wait_Edit()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SelectSongScene");
    }

    IEnumerator Wait_Tutorial()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TutorialScene");
    }

    IEnumerator Wait_Option()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("OptionScene");
    }

    IEnumerator Wait_KeyConfig()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("KeyConfigScene");
    }
}