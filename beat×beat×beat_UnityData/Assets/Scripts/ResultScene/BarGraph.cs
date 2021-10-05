using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarGraph : MonoBehaviour
{
    // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
    public GameObject[] barGraphObject;

    private float maxScale = 80.0f;
    private float yScale = 1.0f;
    private float zScale = 3.0f;

    void Start()
    {
        int sum = 0;
        float[] scale = new float[barGraphObject.Length];


        for (int i = 0; i < scale.Length; i++)
        {
            // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
            sum += JudgeDisplay.count[i];
        }

        if (sum != 0)
        {
            for (int i = 0; i < scale.Length; i++)
            {
                // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
                scale[i] = maxScale * JudgeDisplay.count[i] / sum;
            }
        }

        for (int i = 0; i < scale.Length; i++)
        {
            // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
            barGraphObject[i].transform.localScale = new Vector3(scale[i], yScale, zScale);
        }
    }
}
