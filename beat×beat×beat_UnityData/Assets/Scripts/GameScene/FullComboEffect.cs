using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullComboEffect : MonoBehaviour
{
    public GameObject fullComboText;
    public GameObject fullComboPlane;
    Outline outline;
    Renderer materialRenderer;
    private float pi = 0.0f;
    private float piDelta = 1.5f;
    private float moveWidth = 20.0f;
    private float moveDelta = 10.0f;
    public static bool fullComboFlag;

    void Start()
    {
        fullComboFlag = false;
        outline = fullComboText.GetComponent<Outline>();
        materialRenderer = fullComboPlane.GetComponent<Renderer>();
    }

    void Update()
    {
        if (fullComboFlag)
        {
            if (0.0f <= moveWidth)
            {
                fullComboText.gameObject.SetActive(true);
                outline.effectDistance = new Vector2(moveWidth * Mathf.Sin(pi), 1);
                pi += piDelta * Mathf.PI * Time.deltaTime;
                // piが一周（2π）を超えたら移動幅を減衰
                if (2 * Mathf.PI <= pi)
                {
                    pi -= 2 * Mathf.PI;
                    moveWidth -= moveDelta;
                }
            }

            fullComboPlane.gameObject.SetActive(true);
            if (0 < materialRenderer.material.color.a)
            {
                materialRenderer.material.color -= new Color(0, 0, 0, 0.01f);
                if (materialRenderer.material.color.a < 0)
                {
                    Color color = materialRenderer.material.color;
                    color.a = 0.0f;
                    materialRenderer.material.color = new Color(color.r, color.g, color.b, 0.0f);
                }
            }
        }
    }
}
