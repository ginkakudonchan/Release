// Unityでフェードイン／フェードアウトを実装する方法
// https://tama-lab.net/2017/07/unity%E3%81%A7%E3%83%95%E3%82%A7%E3%83%BC%E3%83%89%E3%82%A4%E3%83%B3%EF%BC%8F%E3%83%95%E3%82%A7%E3%83%BC%E3%83%89%E3%82%A2%E3%82%A6%E3%83%88%E3%82%92%E5%AE%9F%E8%A3%85%E3%81%99%E3%82%8B%E6%96%B9/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    private Image fadeImage;
    float red, green, blue, alfa;
    bool fadeInFlag = false;
    public static bool fadeOutFlag;
    float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理

    void Start()
    {
        fadeOutFlag = false;

        fadeImage = GameObject.Find("Canvas2/FadeObject/Fade").GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
        StartFadeIn();
    }

    void Update()
    {
        if (fadeInFlag)
        {
            FadeIn();

        }

        if (fadeOutFlag)
        {
            FadeOut();
        }
    }

    void StartFadeIn()
    {
        fadeInFlag = true;
    }

    void FadeIn()
    {
        alfa -= fadeSpeed;         // b)透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (alfa < 0)
        {             // d)完全に透明になったら処理を抜ける
            fadeInFlag = false;
            fadeImage.gameObject.SetActive(false);
        }

        void SetAlpha()
        {
            fadeImage.color = new Color(red, green, blue, alfa);
        }
    }

    void FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        alfa += fadeSpeed;         // b)不透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (1 < alfa)
        {             // d)完全に不透明になったら処理を抜ける
            fadeOutFlag = false;
        }

        void SetAlpha()
        {
            fadeImage.color = new Color(red, green, blue, alfa);
        }
    }
}
