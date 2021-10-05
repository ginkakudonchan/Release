using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_KeyConfig : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Wait_Title());
        }
        /*
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSceneOption();
        }
        */
    }

    public static void ChangeSceneTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void ChangeSceneOption()
    {
        FadeScript.fadeOutFlag = true;
        StartCoroutine(Wait_Option());
    }

    IEnumerator Wait_Title()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Wait_Option()
    {
        FadeScript.fadeOutFlag = true;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("OptionScene");
    }
}
