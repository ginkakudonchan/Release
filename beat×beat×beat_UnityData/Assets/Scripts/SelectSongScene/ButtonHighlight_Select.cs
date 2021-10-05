// Unityで選択されているボタンのテキストを点滅 + 簡単なエフェクトを追加
// https://www.kuroshum.com/entry/2018/09/01/Unity%E3%81%A7%E9%81%B8%E6%8A%9E%E3%81%95%E3%82%8C%E3%81%A6%E3%81%84%E3%82%8B%E3%83%9C%E3%82%BF%E3%83%B3%E3%81%AE%E3%83%86%E3%82%AD%E3%82%B9%E3%83%88%E3%82%92%E7%82%B9%E6%BB%85_%2B_%E7%B0%A1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight_Select : MonoBehaviour
{
    // 現在選択中のボタン
    private GameObject Button;

    // 現在の一つ前に選択していたボタン
    private GameObject ButtonBuf;

    // ボタンに付加するエフェクト
    private GameObject[] ButtonEffect = new GameObject[3];
    public static Text buttonText;

    void Update()
    {
        // 現在の選択されているボタンを取得し、Buttonに代入
        Button = EventSystem.current.currentSelectedGameObject;
        // Buttonがnullじゃない時（FadeIn、Outの時nullを出さないように）
        if (Button)
        {
            // 現在の選択されているボタンが前に選択されているボタンと違う場合(選択肢を移動させた場合)
            // 現在のボタンのエフェクトを表示させる
            // 一つ前に選択していたボタンのエフェクトを非表示にさせる
            if (Button != ButtonBuf)
            {
                int j = 0;
                // 選択中のボタンの子要素を取得
                foreach (Transform child in Button.transform)
                {
                    ButtonEffect[j] = child.gameObject;
                    if (j > 0) ButtonEffect[j].SetActive(true);
                    j++;
                }
                j = 0;

                // 一つ前に選択していたボタンのエフェクトを非表示にさせる
                // ButtonBufがNullじゃない時に動作(1回目とかは前回の値が無いため)
                if (ButtonBuf)
                {
                    // 一つ前に選択していたボタンの子要素を取得 
                    foreach (Transform child in ButtonBuf.transform)
                    {
                        ButtonEffect[j] = child.gameObject;
                        if (j > 0) ButtonEffect[j].SetActive(false);
                        j++;
                    }

                    // 選択されているボタンが中央に来るようにする
                    float diffPosY = Button.transform.position.y - ButtonBuf.transform.position.y;
                    // キャンバス上の一番上の子オブジェクトボタンを選択
                    Button[] button = GameObject.Find("Canvas").GetComponentsInChildren<Button>();
                    for (var i = 0; i < button.Length; i++)
                    {
                        button[i].transform.position = new Vector3(
                            button[i].transform.position.x,
                            button[i].transform.position.y - diffPosY,
                            button[i].transform.position.z
                        );
                    }
                }
            }

            // 現在選択されているボタンをButtonBufに代入(現在のボタンを一つ前に選択していたボタンに設定する)
            ButtonBuf = Button;
        }
    }
}
