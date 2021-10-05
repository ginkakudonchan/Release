using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Option : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeSceneTitle();
        }
    }

    void ChangeSceneTitle()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Title());
    }

    void ChangeSceneOption()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Option());
    }
    void ChangeSceneKeyConfig()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_KeyConfig());
    }

    IEnumerator Wait_Title()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
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
