using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    Button button;

    private bool firstGameFlag;

    public static bool gameSceneFlag;
    public static bool editSceneFlag;

    void Start()
    {
        firstGameFlag = JsonFirstGameFlag.ReadFirstGameFlagJson();

        if (firstGameFlag)
        {
            Time.timeScale = 1f;

            // static変数の初期化
            gameSceneFlag = false;
            editSceneFlag = false;

            button = GameObject.Find("Canvas/GameScene").GetComponent<Button>();
            //ボタンが選択された状態になる
            button.Select();


            // ボタンが一番左から一番右(逆も可)に行く時の処理、つまりナビゲーションの自動化
            // 子のボタンやスライダーなど操作可能なコンポーネントを取得する
            var selects = GameObject.Find("Canvas").GetComponentsInChildren<Selectable>();
            for (var i = 0; i < selects.Length; i++)
            {
                var nav = selects[i].navigation;
                nav.mode = Navigation.Mode.Explicit;
                nav.selectOnLeft = selects[i == 0 ? selects.Length - 1 : i - 1];
                nav.selectOnRight = selects[(i + 1) % selects.Length];
                selects[i].navigation = nav;
            }
        }
        else
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }
}