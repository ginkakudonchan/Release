using UnityEngine;
using System.Collections;

public class ComboEffect : SingletonMonoBehaviour<ComboEffect>
{
    public GameObject[] _comboEffects;
    private bool flag = false;
    private Renderer[] materialRenderer;
    private Color[] color;
    private int _num = 0;
    private float pi = 0.0f;
    private float effectTime = 1.0f;

    void Start()
    {
        materialRenderer = new Renderer[3];
        color = new Color[3];
        for (int i = 0; i < 3; i++)
        {
            materialRenderer[i] = _comboEffects[i].GetComponent<Renderer>();
            color[i] = materialRenderer[i].material.color;
        }
    }

    void Update()
    {
        // ComboEffectがアクティブ状態の時、透明度を変化させる
        if (flag)
        {
            // color.aの値を変化させることで透明度を変化させられる
            Color colorbuf = materialRenderer[_num].material.color;
            materialRenderer[_num].material.color = new Color(colorbuf.r, colorbuf.g, colorbuf.b, color[_num].a * Mathf.Sin(pi));
            pi += Mathf.PI / effectTime * Time.deltaTime;
        }
    }


    // 0:S_Critical, 1:CriticalNear, 2:Error, 3:Air
    public void PlayEffect(int num)
    {
        StartCoroutine(TouchEffect(num));
    }

    IEnumerator TouchEffect(int num)
    {
        _comboEffects[num].SetActive(true);
        flag = true;
        pi = 0.0f;
        _num = num;

        yield return new WaitForSeconds(effectTime);
        _comboEffects[num].SetActive(false);
        flag = false;
        materialRenderer[num].material.color = color[num];
    }
}
