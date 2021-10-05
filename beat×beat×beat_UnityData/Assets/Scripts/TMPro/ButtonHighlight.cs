// Unityで選択されているボタンのテキストを点滅 + 簡単なエフェクトを追加
// https://www.kuroshum.com/entry/2018/09/01/Unity%E3%81%A7%E9%81%B8%E6%8A%9E%E3%81%95%E3%82%8C%E3%81%A6%E3%81%84%E3%82%8B%E3%83%9C%E3%82%BF%E3%83%B3%E3%81%AE%E3%83%86%E3%82%AD%E3%82%B9%E3%83%88%E3%82%92%E7%82%B9%E6%BB%85_%2B_%E7%B0%A1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHighlight : MonoBehaviour
{
    /*
     * 現在選択中のボタン
     */
    private GameObject Button;
    /*
     * 現在の一つ前に選択していたボタン
     */
    private GameObject ButtonBuf;
    /*
     * ボタンに付加するエフェクト
     */
    private GameObject[] ButtonEffect = new GameObject[3];
    private int i = 0;

    void Update()
    {
        // 現在の選択されているボタンを取得し、Buttonに代入
        Button = EventSystem.current.currentSelectedGameObject;
        // Buttonがnullじゃない時（FadeIn、Outの時nullを出さないように）
        if (Button)
        {
            // updateの1回目だけ行う処理(Startの時に選択されているボタンが取得できなかったため)
            // ButtonBufに現在の選択されているボタンを代入(初期化)
            if (i == 0)
            {
                ButtonBuf = Button;
                i++;
            }
            // 現在の選択されているボタンが前に選択されているボタンと違う場合(選択肢を移動させた場合)
            // 現在のボタンのエフェクトを表示させる
            //  一つ前に選択していたボタンのエフェクトを非表示にさせる

            if (Button != ButtonBuf)
            {
                int j = 0;
                // 選択中のボタンの子要素を取得(1番目の要素は文字なので飛ばす)
                foreach (Transform child in Button.transform)
                {
                    ButtonEffect[j] = child.gameObject;
                    if (j > 0) ButtonEffect[j].SetActive(true);
                    j++;
                }

                // テキストのエフェクトをONにする
                ButtonEffect[0].GetComponent<TextHighlight>().enabled = true;

                j = 0;
                // 一つ前に選択していたボタンの子要素を取得(1番目の要素は文字なので飛ばす)
                foreach (Transform child in ButtonBuf.transform)
                {
                    ButtonEffect[j] = child.gameObject;
                    if (j > 0) ButtonEffect[j].SetActive(false);
                    j++;
                }

                // テキストのエフェクトをOFFにする
                ButtonEffect[0].GetComponent<TextHighlight>().enabled = false;

                //テキストのエフェクトを普通の状態(0)に戻す
                TextMeshProUGUI tmPro = ButtonEffect[0].GetComponent<TextMeshProUGUI>();
                Material material = tmPro.fontMaterial;
                material.SetFloat("_OutlineWidth", 0);
            }

            // 現在選択されているボタンをButtonBufに代入(現在のボタンを一つ前に選択していたボタンに設定する)
            ButtonBuf = Button;
        }
    }
}